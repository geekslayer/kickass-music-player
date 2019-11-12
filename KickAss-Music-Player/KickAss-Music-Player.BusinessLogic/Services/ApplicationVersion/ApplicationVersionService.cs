using KickAss_Music_Player.DataModels.Dto.ApplicationVersion.Results;
using System;
using System.Threading.Tasks;

namespace KickAss_Music_Player.BusinessLogic.Services.ApplicationVersion
{
    public class ApplicationVersionService : ServiceBase, IApplicationVersionService
    {
        public async Task<ApplicationVersionResult> GetApplicationVersion()
        {
            return await Task.FromResult(new ApplicationVersionResult { Success = true, Data = Convert.ToString("0.1.0") }) ;
        }
    }
}
