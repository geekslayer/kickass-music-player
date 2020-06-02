using KickAss_Music_Player.Security;
using Microsoft.AspNetCore.Mvc;

namespace KickAss_Music_Player.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Services context
        /// </summary>
        //protected readonly IServicesContext ServicesContext;

        /// <summary>
        /// Token information processor
        /// </summary>
        protected readonly ITokenInterpretor TokenInterpretor;

        /// <summary>
        /// Protected constructor that is used by child classes
        /// </summary>
        /// <param name="services">Service context</param>
        /// <param name="logger">Logger (NLog)</param>
        /// <param name="tokenInterpretor">Token information processor</param>
        protected BaseController(/*IServicesContext services,*/ ITokenInterpretor tokenInterpretor)
        {
            //ServicesContext = services;
            TokenInterpretor = tokenInterpretor;
        }
    }
}
