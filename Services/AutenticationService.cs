using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.ProtectedBrowserStorage;

namespace OpoTest.Services
{
    public class AutenticationService : AuthenticationStateProvider
    {

        private ProtectedSessionStorage protectedSessionStorage;

        public AutenticationService(ProtectedSessionStorage protectedSessionStorage)
        {
            this.protectedSessionStorage = protectedSessionStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string username = await protectedSessionStorage.GetAsync<string>("CurrentUser");
                ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username), }, "simpleAutentication"));
                return new AuthenticationState(user);
            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        public async Task Autenticate(string username, string password)
        {
            if (password == "aroa2294")
            {
                await protectedSessionStorage.SetAsync("CurrentUser", username);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        public async Task Exit()
        {
            await protectedSessionStorage.DeleteAsync("CurrentUser");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
