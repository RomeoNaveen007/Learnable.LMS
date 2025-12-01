using Learnable.Application.Common.Dtos;
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
        public async Task<IActionResult> GlobalSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<GlobalSearchDto>());

            query = query.ToLower();

            var users = await _context.Users
                .Where(x => x.Username.ToLower().Contains(query))
                .Select(x => new GlobalSearchDto
                {
                    Id = x.UserId,
                    Type = "User",
                    Title = x.Username,
                    SubTitle = x.Role
                })
                .ToListAsync();

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

            var result = users.Concat(classes).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Student)
                    .ThenInclude(s => s.User) // adjust names per your schema
                .Include(u => u.ClassStudents)
                    .ThenInclude(cs => cs.Class) // class navigation
                .Include(u => u.Teacher)
                    .ThenInclude(t => t.Classes) // teacher created classes nav prop
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            var dto = new UserDetailsDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                ContactNumber = /* map phone if available */ null,
                Bio = /* map bio */ null,
                Role = user.Role
            };

            // Enrolled classes (student)
            var enrolled = await _context.ClassStudents
                .Where(cs => cs.UserId == id)
                .Include(cs => cs.Class)
                .Select(cs => new SimpleClassDto
                {
                    ClassId = cs.Class.ClassId,
                    ClassName = cs.Class.ClassName,
                    Description = cs.Class.Description
                }).ToListAsync();

            dto.EnrolledClasses = enrolled;

            // Created classes (teacher)
            var created = await _context.Classes
                .Where(c => c.Teacher.ProfileId == id) // or however you store teacher FK
                .Select(c => new SimpleClassDto
                {
                    ClassId = c.ClassId,
                    ClassName = c.ClassName,
                    Description = c.Description
                }).ToListAsync();

            dto.CreatedClasses = created;

            return Ok(dto);
        }
    }
}
