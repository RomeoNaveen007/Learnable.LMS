using Learnable.Domain.Common.Email;
using Learnable.Domain.Common.OTP;
using Learnable.Domain.Entities;
using Learnable.Domain.Errors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Learnable.Infrastructure.Persistence.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // ============================================================
        //                        DbSet Properties
        // ============================================================
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClassStudent> ClassStudents { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }
        public virtual DbSet<StudentsAnswer> StudentsAnswers { get; set; }
        public virtual DbSet<Prompt> Prompts { get; set; }
        public virtual DbSet<Repository> Repositories { get; set; }
        public virtual DbSet<RequestNotification> RequestNotifications { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserOtp> UserOtps { get; set; }
        public virtual DbSet<SmtpSetting> SmtpSettings { get; set; }
        public virtual DbSet<ApiException> ApiExceptions { get; set; }

        // ============================================================
        //                        Database Config
        // ============================================================
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDb;" +
                "Initial Catalog=Learnable_DB;" +
                "Integrated Security=True;" +
                "MultipleActiveResultSets=True;" +
                "TrustServerCertificate=True;"
            );

        // ============================================================
        //                    Model Creating (Fluent API)
        // ============================================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var jsonOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = false };

            // -------------------- Asset --------------------
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.AssetsProfileId);
                entity.Property(e => e.AssetsProfileId).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Repo)
                      .WithMany(p => p.Assets);
            });

            // -------------------- AuditLog --------------------
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogId).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Class)
                      .WithMany(p => p.AuditLogs);

                entity.HasOne(d => d.User)
                      .WithMany(p => p.AuditLogs);
            });

            // -------------------- Class --------------------
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.ClassId);
                entity.Property(e => e.ClassId).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Status).HasDefaultValue("Active");

                entity.HasOne(d => d.Teacher)
                      .WithMany(p => p.Classes);
            });

            // -------------------- ClassStudent --------------------
            modelBuilder.Entity<ClassStudent>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.UserId });

                entity.Property(e => e.JoinDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.StudentStatus).HasDefaultValue("Active");

                entity.HasOne(d => d.Class)
                      .WithMany(p => p.ClassStudents)
                      .HasForeignKey(d => d.ClassId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                      .WithMany(p => p.ClassStudents)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // -------------------- Exam --------------------
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasKey(e => e.ExamId);
                entity.Property(e => e.ExamId).ValueGeneratedNever();

                entity.HasOne(d => d.Repo)
                      .WithMany(p => p.Exams);

                // Exam delete -> Question delete (Cascade)
                entity.HasMany(d => d.Questions)
                      .WithOne(q => q.Exam)
                      .HasForeignKey(q => q.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------- ExamQuestion --------------------
            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.HasKey(e => e.QuestionId);
                entity.Property(e => e.QuestionId).ValueGeneratedNever();

                entity.Property(e => e.Question).IsRequired();

                entity.Property(e => e.Answers)
                      .HasConversion(
                          v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                          v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>())
                      .HasColumnType("nvarchar(max)");

                entity.Property(e => e.CorrectAnswerIndex).IsRequired();

                entity.HasOne(d => d.Exam)
                      .WithMany(p => p.Questions)
                      .HasForeignKey(d => d.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------- Mark (UPDATED) --------------------
            modelBuilder.Entity<Mark>(entity =>
            {
                // 🔥 NEW: Single Primary Key from the updated Entity
                entity.HasKey(e => e.MarkId);
                entity.Property(e => e.MarkId).ValueGeneratedNever();

                // 🔥 NEW: Ensure uniqueness for Student+Exam and allow StudentsAnswer to link
                // This creates a Unique Constraint on ExamId + StudentId
                entity.HasAlternateKey(e => new { e.ExamId, e.StudentId });

                // Relation to User (Student) -> User Delete = Mark Delete (Cascade)
                entity.HasOne(d => d.User)
                      .WithMany(p => p.Marks)
                      .HasForeignKey(d => d.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relation to Exam -> Exam Delete = Prevent Delete if Marks exist (Restrict)
                // Note: Can't use ClientSetNull because ExamId is non-nullable Guid.
                entity.HasOne(d => d.Exam)
                      .WithMany(p => p.Marks)
                      .HasForeignKey(d => d.ExamId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // -------------------- StudentsAnswer --------------------
            modelBuilder.Entity<StudentsAnswer>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Relation to Mark (UPDATED to use Principal Key)
                // Links StudentsAnswer to Mark via ExamId + StudentId
                entity.HasOne(e => e.Mark)
                      .WithMany(m => m.StudentsAnswers)
                      .HasForeignKey(e => new { e.ExamId, e.StudentId }) // FK in StudentsAnswer
                      .HasPrincipalKey(m => new { m.ExamId, m.StudentId }) // Points to Alternate Key in Mark
                      .OnDelete(DeleteBehavior.Cascade); // Mark Delete = Answer Delete

                // Relation to Question
                // Exam (Question) delete -> Answer stays (ClientSetNull)
                entity.HasOne(e => e.Question)
                      .WithMany()
                      .HasForeignKey(e => e.QuestionId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.SubmittedAt)
                      .HasDefaultValueSql("(getdate())");
            });

            // -------------------- Prompt --------------------
            modelBuilder.Entity<Prompt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            // -------------------- Repository --------------------
            modelBuilder.Entity<Repository>(entity =>
            {
                entity.HasKey(e => e.RepoId);
                entity.Property(e => e.RepoId).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Status).HasDefaultValue("Active");

                entity.HasOne(d => d.Class)
                      .WithMany(p => p.Repositories);
            });

            // -------------------- RequestNotification --------------------
            modelBuilder.Entity<RequestNotification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);
                entity.Property(e => e.NotificationId).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.NotificationStatus).HasDefaultValue("Sent");

                entity.HasOne(d => d.Receiver)
                      .WithMany(p => p.RequestNotificationReceivers)
                      .HasForeignKey(d => d.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Sender)
                      .WithMany(p => p.RequestNotificationSenders)
                      .HasForeignKey(d => d.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Class)
                      .WithMany(c => c.RequestNotifications)
                      .HasForeignKey(d => d.ClassId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------- Teacher --------------------
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.ProfileId);
                entity.Property(e => e.ProfileId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                      .WithOne(p => p.Teacher);
            });

            // -------------------- User --------------------
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // -------------------- SmtpSetting --------------------
            modelBuilder.Entity<SmtpSetting>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Host).HasMaxLength(200);
                entity.Property(e => e.Username).HasMaxLength(200);
                entity.Property(e => e.Password).HasMaxLength(200);
                entity.Property(e => e.FromName).HasMaxLength(100);
                entity.Property(e => e.FromEmail).HasMaxLength(150);
            });

            // -------------------- UserOtp --------------------
            modelBuilder.Entity<UserOtp>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.OtpCode).HasMaxLength(10);

                entity.HasIndex(e => e.Email)
                      .HasDatabaseName("IX_UserOtp_Email");
            });

            // -------------------- ApiException --------------------
            modelBuilder.Entity<ApiException>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Message)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(e => e.Details)
                      .HasMaxLength(2000);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}