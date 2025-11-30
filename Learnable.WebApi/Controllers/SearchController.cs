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

        // Constructor injection
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GlobalSearch")]
        public async Task<IActionResult> GlobalSearch(string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query is required.");

            query = query.ToLower();

            var users = await _context.Users
                .Where(x => x.Username.ToLower().Contains(query)
                         || x.Email.ToLower().Contains(query))
                .Select(x => new GlobalSearchDto
                {
                    Id = x.UserId,
                    Type = "User",
                    Title = x.Username,
                    SubTitle = x.Email
                }).ToListAsync();

            var classes = await _context.Classes
                .Where(x => x.ClassName.ToLower().Contains(query))
                .Select(x => new GlobalSearchDto
                {
                    Id = x.ClassId,
                    Type = "Class",
                    Title = x.ClassName,
                    SubTitle = ""
                }).ToListAsync();

            return Ok(users.Concat(classes));
        }

    }
}
