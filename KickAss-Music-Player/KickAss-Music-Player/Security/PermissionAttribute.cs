using KickAss_Music_Player.Controllers;
using KickAss_Music_Player.DataModels.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KickAss_Music_Player.Security
{
    /// <summary>
    ///     Validate the permission of the user before to access the Controller or the Action.
    ///     Add a 401 response into the response when not authorized.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    public class PermissionAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PermissionAttribute" /> class.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public PermissionAttribute(PermissionType permission)
        {
            Permission = permission;
        }

        /// <summary>
        ///     Gets or sets the permission
        /// </summary>
        public PermissionType Permission { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!HasPermission(filterContext.HttpContext.User))
            {
                HandleUnauthorizedAction(filterContext);
                return;
            }

            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// Indicate if the user has the permission.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool HasPermission(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return false;

            if (this.Permission == PermissionType.AllAuthenticated)
                return true;

            var ti = new TokenInterpretor(null);
            ti.InitUser(user);
            var isAuthorized = ti.IsAuthorized(Permission);
            return isAuthorized;
        }

        private void HandleUnauthorizedAction(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var tokenInterpretor = new TokenInterpretor(null);
                tokenInterpretor.InitUser(filterContext.HttpContext.User);
                var username = tokenInterpretor.GetFirstClaimValue(ClaimConstants.UserNameClaimName);
                var msg = $"{username} doesn't have the {this.Permission.ToString()} permission";
                filterContext.Result = new ContentResult()
                {
                    Content = msg
                };
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden; // 403 

                //logger.Error(msg);
            }
            else
            {
                filterContext.Result = new ContentResult()
                {
                    Content = HttpStatusCode.Unauthorized.ToString()
                };
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401

                //logger.Warn($"Unauthorized access attempt for \"{Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(filterContext.HttpContext.Request)}\" by {filterContext.HttpContext.Connection.RemoteIpAddress}");
            }
        }
    }
}
