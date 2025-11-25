using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Users;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Learnable.Infrastructure.Persistence.Data
{
    public class Seed
    {
        public static async Task SeedUser(ApplicationDbContext context, IUnitOfWork unitOfWork, IPasswordService password)
        {
            if (await context.Users.AnyAsync()) return;

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Persistence", "Data", "UserSeedData.json");

            if (!File.Exists(path))
            {
                Console.WriteLine($"❌ Seed file not found at: {path}");
                return;
            }

            var userData = await File.ReadAllTextAsync(path);
            var users = JsonSerializer.Deserialize<List<UserDto>>(userData);

            if (users == null || users.Count == 0)
            {
                Console.WriteLine("❌ No users found in seed file.");
                return;
            }

            // -------------------------------
            // Ensure default teacher exists
            // -------------------------------
            var defaultTeacher = await context.Teachers.FirstOrDefaultAsync();
            if (defaultTeacher == null)
            {
                password.CreatePasswordHash("Pa$$w0rd", out string hash, out string salt);
                var defaultUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "default.teacher",
                    Email = "teacher@learnable.com",
                    Role = "Teacher",
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    DisplayName = "Default Teacher",
                    FullName = "Default Teacher",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                context.Users.Add(defaultUser);

                defaultTeacher = new Teacher
                {
                    ProfileId = Guid.NewGuid(),
                    UserId = defaultUser.UserId,
                    DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)),
                    ContactPhone = "0000000000",
                    Bio = "Default teacher for seeding",
                    LastUpdatedAt = DateTime.UtcNow
                };
                context.Teachers.Add(defaultTeacher);
                await unitOfWork.SaveChangesAsync();
            }

            // -------------------------------
            // Ensure default class exists
            // -------------------------------
            var defaultClassName = "General Class";
            var defaultClass = await context.Classes.FirstOrDefaultAsync(c => c.ClassName == defaultClassName);

            if (defaultClass == null)
            {
                defaultClass = new Class
                {
                    ClassId = Guid.NewGuid(),
                    TeacherId = defaultTeacher.ProfileId,
                    ClassName = defaultClassName,
                    ClassJoinName = "GENERAL-001", // must be unique & NOT NULL
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active"
                };
                context.Classes.Add(defaultClass);
                await unitOfWork.SaveChangesAsync();
            }

            // -------------------------------
            // Seed default users/students
            // -------------------------------
            foreach (var u in users)
            {
                password.CreatePasswordHash("Pa$$w0rd", out string hash, out string salt);

                var user = new User
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Username = u.Username,
                    DisplayName = u.DisplayName,
                    FullName = u.FullName,
                    Role = u.Role,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                context.Users.Add(user);
                Console.WriteLine($"✔ Seeding user: {u.Username}");

                // --- Auto-join default class if Student ---
                if (user.Role == "Student")
                {
                    bool alreadyJoined = await context.ClassStudents
                        .AnyAsync(cs => cs.ClassId == defaultClass.ClassId && cs.UserId == user.UserId);

                    if (!alreadyJoined)
                    {
                        context.ClassStudents.Add(new ClassStudent
                        {
                            ClassId = defaultClass.ClassId,
                            UserId = user.UserId,
                            JoinDate = DateTime.UtcNow,
                            StudentStatus = "Active"
                        });
                    }
                }
            }

            await unitOfWork.SaveChangesAsync();
            Console.WriteLine("✔ All users seeded and joined to default class successfully.");
        }
    }
}