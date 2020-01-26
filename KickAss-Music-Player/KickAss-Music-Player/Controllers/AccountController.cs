using KickAss_Music_Player.DataModels.Dto.Account;
using KickAss_Music_Player.DataModels.Dto.Security;
using KickAss_Music_Player.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KickAss_Music_Player.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        protected AccountController(ITokenInterpretor tokenInterpretor) : base(tokenInterpretor)
        {
        }

        [HttpPost]
        [Route("Login")]
        public async Task<AccountResult> Login([FromBody] LoginCommand loginCommand)
        {
            return await Task.FromResult(new AccountResult());
        }

        public async Task<string> Get()
        {
            return await Task.FromResult("GoGoGo!!!");
        }

    }
}
