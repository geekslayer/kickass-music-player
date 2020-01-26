using KickAss_Music_Player.DataModels.Dto.Security;
using KickAss_Music_Player.DataModels.Dto.User;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace KickAss_Music_Player.Security
{
    public interface IOpenIdRestApiAuthenticator
    {
        Task<UserResult> AuthenticateAsync(HttpContext httpContext, LoginCommand loginCommand);
        Task<UserResult> RefreshTokenAsync(HttpContext httpContext);
    }
}
