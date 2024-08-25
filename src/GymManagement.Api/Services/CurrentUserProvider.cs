using GymManagement.Application.Common.Interfaces;

namespace GymManagement.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUserProvider GetCurrentUser()
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();

        var claims = _httpContextAccessor.HttpContext.User.Claims
            .First(claim => claim.Type == "id");

        return new CurrentUser(Guid.Parse(claims.Value));
    }
}
