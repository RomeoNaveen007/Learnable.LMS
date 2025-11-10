using Learnable.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Persistence.Data
{
    internal partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Asset> Assets { get; set; }

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public virtual DbSet<Class> Classes { get; set; }

        public virtual DbSet<ClassStudent> ClassStudents { get; set; }

        public virtual DbSet<Exam> Exams { get; set; }

        public virtual DbSet<Mark> Marks { get; set; }

        public virtual DbSet<Prompt> Prompts { get; set; }

        public virtual DbSet<Repository> Repositories { get; set; }

        public virtual DbSet<RequestNotification> RequestNotifications { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<Teacher> Teachers { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDb; Initial Catalog=Learnable_DB; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.AssetsProfileId).HasName("PK__Assets__278E59AF58F39BA2");

                entity.Property(e => e.AssetsProfileId).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Repo).WithMany(p => p.Assets).HasConstraintName("FK__Assets__RepoId__440B1D61");
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.LogId).HasName("PK__AuditLog__5E54864800A75C89");

                entity.Property(e => e.LogId).ValueGeneratedNever();
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Class).WithMany(p => p.AuditLogs).HasConstraintName("FK__AuditLog__ClassI__4F7CD00D");

                entity.HasOne(d => d.User).WithMany(p => p.AuditLogs).HasConstraintName("FK__AuditLog__UserId__4E88ABD4");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.ClassId).HasName("PK__Class__CB1927C0735CA6EB");

                entity.Property(e => e.ClassId).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Status).HasDefaultValue("Active");

                entity.HasOne(d => d.Teacher).WithMany(p => p.Classes).HasConstraintName("FK__Class__TeacherId__35BCFE0A");
            });

            modelBuilder.Entity<ClassStudent>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.StudentId }).HasName("PK__ClassStu__48357579EC94B403");

                entity.Property(e => e.JoinDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.StudentStatus).HasDefaultValue("Active");

                entity.HasOne(d => d.Class).WithMany(p => p.ClassStudents)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClassStud__Class__3A81B327");

                entity.HasOne(d => d.Student).WithMany(p => p.ClassStudents)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClassStud__Stude__3B75D760");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasKey(e => e.ExamId).HasName("PK__Exam__297521C7F8D3E0FA");

                entity.Property(e => e.ExamId).ValueGeneratedNever();

                entity.HasOne(d => d.Repo).WithMany(p => p.Exams).HasConstraintName("FK__Exam__RepoId__46E78A0C");
            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.HasKey(e => new { e.ExamId, e.StudentId }).HasName("PK__Marks__AA59737E7EEE6756");

                entity.HasOne(d => d.Exam).WithMany(p => p.Marks)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Marks__ExamId__49C3F6B7");

                entity.HasOne(d => d.Student).WithMany(p => p.Marks)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Marks__StudentId__4AB81AF0");
            });

            modelBuilder.Entity<Prompt>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Prompt__3214EC0795D7F6C9");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Repository>(entity =>
            {
                entity.HasKey(e => e.RepoId).HasName("PK__Reposito__856806780424CB75");

                entity.Property(e => e.RepoId).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Status).HasDefaultValue("Active");

                entity.HasOne(d => d.Class).WithMany(p => p.Repositories).HasConstraintName("FK__Repositor__Class__403A8C7D");
            });

            modelBuilder.Entity<RequestNotification>(entity =>
            {
                entity.HasKey(e => e.NotificationId).HasName("PK__RequestN__20CF2E128EDF9FB2");

                entity.Property(e => e.NotificationId).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.NotificationStatus).HasDefaultValue("Sent");

                entity.HasOne(d => d.Receiver).WithMany(p => p.RequestNotificationReceivers).HasConstraintName("FK__RequestNo__Recei__5535A963");

                entity.HasOne(d => d.Sender).WithMany(p => p.RequestNotificationSenders).HasConstraintName("FK__RequestNo__Sende__5441852A");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52B996171B967");

                entity.Property(e => e.StudentId).ValueGeneratedNever();
                entity.Property(e => e.EnrollmentDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User).WithOne(p => p.Student).HasConstraintName("FK__Student__UserId__300424B4");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.ProfileId).HasName("PK__Teacher__290C88E41AE75428");

                entity.Property(e => e.ProfileId).ValueGeneratedNever();

                entity.HasOne(d => d.User).WithOne(p => p.Teacher).HasConstraintName("FK__Teacher__UserId__2B3F6F97");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CB2415AFE");

                entity.Property(e => e.UserId).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
