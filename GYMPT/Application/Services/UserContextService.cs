using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GYMPT.Infrastructure.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId != null ? int.Parse(userId) : null;
        }

        public string? GetUserRole()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
        }

        public string? GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.Name;
        }
    }
}
