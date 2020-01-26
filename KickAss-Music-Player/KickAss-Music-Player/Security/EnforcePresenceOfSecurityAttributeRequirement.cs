using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickAss_Music_Player.Security
{
    /// This class is used in XML comment below. Otherwise generates a warning.
    internal class XmlCommentCrefClass2 : AuthorizationHandler<EnforcePresenceOfSecurityAttributeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EnforcePresenceOfSecurityAttributeRequirement requirement)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Insure that all web method are not accessible until you define the allowed security attributes.
    /// </summary>
    /// <seealso cref="XmlCommentCrefClass2" />
    /// <seealso cref="Microsoft.AspNetCore.Authorization.IAuthorizationRequirement" />
    public class EnforcePresenceOfSecurityAttributeRequirement : AuthorizationHandler<EnforcePresenceOfSecurityAttributeRequirement>, IAuthorizationRequirement
    {
        private readonly IAuthenticationSetting _authenticationSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnforcePresenceOfSecurityAttributeRequirement"/> class.
        /// </summary>
        /// <param name="authenticationSetting">The authentication setting.</param>
        public EnforcePresenceOfSecurityAttributeRequirement(IAuthenticationSetting authenticationSetting)
        {
            _authenticationSetting = authenticationSetting;
        }

        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EnforcePresenceOfSecurityAttributeRequirement requirement)
        {
            var mvcContext = context.Resource as
                Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;

            if (mvcContext == null || !_authenticationSetting.EnforcePresenceOfSecurityAttribute)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var action = mvcContext.ActionDescriptor as ControllerActionDescriptor;

            string displayUrl = null;
            if (_authenticationSetting.IgnoredPathsOfSecurityAttribute != null)
                displayUrl = mvcContext.HttpContext.Request.GetDisplayUrl();

            // Allow action to be called anonymously
            if (context.User.Identity.IsAuthenticated && AttributeHelper.IsDefined<PermissionAttribute>(action))
                context.Succeed(requirement);
            // Must have PermissionClaimName attribute if AnonymousAttribute is not defined
            else if (AttributeHelper.IsDefined<AllowAnonymousAttribute>(action))
                context.Succeed(requirement);
            else if (_authenticationSetting.IgnoredPathsOfSecurityAttribute != null &&
                     _authenticationSetting.IgnoredPathsOfSecurityAttribute.Any(ignoredPath => displayUrl.StartsWith(ignoredPath)))
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
