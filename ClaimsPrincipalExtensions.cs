using System.Security.Claims;

namespace ASC.Utilities
{
    public static class ClaimsPrincipalExtensions
    {
        public static CurrentUser GetCurrentUser(this ClaimsPrincipal principal)
        {
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return new CurrentUser { IsAuthenticated = false };
            }

            return new CurrentUser
            {
                IsAuthenticated = true,
                UserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                UserName = principal.Identity.Name ?? string.Empty,
                Role = principal.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty
            };
        }
    }
}