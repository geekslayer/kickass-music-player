using KickAss_Music_Player.BusinessLogic.Services.ApplicationVersion;
using KickAss_Music_Player.DataModels.Dto.ApplicationVersion.Results;
using KickAss_Music_Player.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KickAss_Music_Player.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class ApplicationVersionController : BaseController
    {
        private readonly IApplicationVersionService _applicationVersionService;

        private readonly ILogger<ApplicationVersionController> _logger;

        public ApplicationVersionController(ILogger<ApplicationVersionController> logger, IApplicationVersionService applicationVersionService, ITokenInterpretor tokenInterpretor) : base(tokenInterpretor)
        {
            _logger = logger;
            _applicationVersionService = applicationVersionService;
        }

        [HttpGet]
        public async Task<ApplicationVersionResult> Get()
        {
            return await _applicationVersionService.GetApplicationVersion();
        }
    }
}
