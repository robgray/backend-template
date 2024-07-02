namespace Api.Infrastructure.User;

using System;
using System.Linq;
using System.Security.Claims;
using Core.Infrastructure.User;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserContext(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
        
        Reset();
    }

    public void Reset()
    {
        if (_httpContextAccessor.HttpContext?.User is null)
        {
            throw new InvalidOperationException("User is not authenticated so we can't access their info");
        }

        var principal = _httpContextAccessor.HttpContext!.User;

        if (!MustHaveClaim(principal, "Sub"))
        {
            throw new InvalidOperationException("ClaimsPrincipal does not have Sub claim");
        }

        InternalId = new Lazy<string>(() => GetIdentifier(principal));  
        
        InternalName = new Lazy<string>(() => GetClaimValue(principal, "Name"));
    }

    public string Name => InternalName.Value;

    public string Id => InternalId.Value;

    private Lazy<string> InternalId { get; set; } = new(() => string.Empty);

    private Lazy<string> InternalName { get; set; } = new(() => string.Empty);
    
    static bool MustHaveClaim(ClaimsPrincipal principal, string claimType) =>
        principal.HasClaim(claimType, "true");

    static string GetClaimValue(ClaimsPrincipal principal, string claimType) =>
        principal.FindFirstValue(claimType) ?? String.Empty;
    
    static string[] GetClaimValues(ClaimsPrincipal principal, string claimType) =>
        principal.Claims
            .Where(c => c.Type == claimType)
            .Select(c => c.Value)
            .ToArray();

    private string GetIdentifier(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(x => x.Type == JwtRegisteredClaimNames.Email))
        {
            return principal.FindFirstValue(JwtRegisteredClaimNames.Email)!;
        }

        if (principal.HasClaim(x => x.Type == JwtRegisteredClaimNames.Sub))
        {
            return principal.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
        }

        return "Unknown";
    }
}
