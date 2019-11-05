using KickAss_Music_Player.DataModels.Dto.ApplicationVersion.Results;
using System.Threading.Tasks;

namespace KickAss_Music_Player.BusinessLogic.Services.ApplicationVersion
{
    public interface IApplicationVersionService
    {
        Task<ApplicationVersionResult> GetApplicationVersion();
    }
}
