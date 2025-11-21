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
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(user);

                Console.WriteLine($"✔ Seeding user: {u.Username}");
            }

            await unitOfWork.SaveChangesAsync();
            Console.WriteLine("✔ All users seeded successfully.");
        }
    }
}