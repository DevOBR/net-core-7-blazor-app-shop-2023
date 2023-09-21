using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AppShop.Web.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonimous = new ClaimsIdentity();
            var adminUser = new ClaimsIdentity(new List<Claim>
            {
                new Claim("FirstName", "Admin"),
                new Claim("LastName", "Admin"),
                new Claim(ClaimTypes.Name, "admin@yopmail.com"),
                new Claim(ClaimTypes.Role, "Admin")
            }
            , authenticationType: "test");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(adminUser)));
        }
    }
}

