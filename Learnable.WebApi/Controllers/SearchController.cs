using Learnable.Application.Common.Dtos;
using Learnable.Domain.Entities;
using Learnable.Infrastructure.Persistence.Data; // DbContext path
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learnable.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GlobalSearch")]
        public async Task<IActionResult> GlobalSearch(string query, string role)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<GlobalSearchDto>());

            query = query.ToLower();
            role = role?.ToLower();

            List<GlobalSearchDto> result = new List<GlobalSearchDto>();

            // ============================
            //   ROLE = STUDENT
            //   → Show Only CLASS Results
            // ============================
            if (role == "student")
            {
                var classes = await _context.Classes
                    .Where(x => x.ClassName.ToLower().Contains(query))
                    .Select(x => new GlobalSearchDto
                    {
                        Id = x.ClassId,
                        Type = "Class",
                        Title = x.ClassName,
                        SubTitle = "Class"
                    })
                    .ToListAsync();

                return Ok(classes); 
            }

           
            if (role == "teacher")
            {
                var students = await _context.Users
                    .Where(u => u.Role.ToLower() == "student" &&
                                u.Username.ToLower().Contains(query))
                    .Select(x => new GlobalSearchDto
                    {
                        Id = x.UserId,
                        Type = "User",
                        Title = x.Username,
                        SubTitle = "Student"
                    })
                    .ToListAsync();

                return Ok(students); 
            }

            var allUsers = await _context.Users
                .Where(x => x.Username.ToLower().Contains(query))
                .Select(x => new GlobalSearchDto
                {
                    Id = x.UserId,
                    Type = "User",
                    Title = x.Username,
                    SubTitle = x.Role
                })
                .ToListAsync();

            var allClasses = await _context.Classes
                .Where(x => x.ClassName.ToLower().Contains(query))
                .Select(x => new GlobalSearchDto
                {
                    Id = x.ClassId,
                    Type = "Class",
                    Title = x.ClassName,
                    SubTitle = "Class"
                })
                .ToListAsync();

            return Ok(allUsers.Concat(allClasses).ToList());
        }

    }
}
