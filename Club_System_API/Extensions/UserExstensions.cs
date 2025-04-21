
using System.Security.Claims;

namespace Club_System_API.Extensions
    {
        public static class UserExstensions
        {
            public static string? GetUserId(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier);

        }
    }
