using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
                throw new Exception("No NameIdentifier claim present (Cannot get UserId)");

            return Guid.Parse(id);
        }
    }
}
