using KickAss_Music_Player.BusinessLogic.Services.User;
using KickAss_Music_Player.DataModels.Dto.Security;
using KickAss_Music_Player.DataModels.Dto.User;
using KickAss_Music_Player.DataModels.Security;
using KickAss_Music_Player.Security.TokenModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KickAss_Music_Player.Security
{
    /// <summary>
    ///     Validate the permissions for a user
    /// </summary>
    public class TokenInterpretor : ITokenInterpretor
    {
        private ClaimsPrincipal _user;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenInterpretor" /> class.
        /// </summary>
        /// <param name="serviceContext">Injecting the serviceContext for the user profile</param>
        public TokenInterpretor(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Init request user
        /// </summary>
        /// <param name="user">The user.</param>
        public void InitUser(ClaimsPrincipal user)
        {
            _user = user;
        }

        /// <summary>
        /// Convert this TokenInterpretor's ClaimsPrincipal into a more usuable user object.
        /// If the token is passed, it will be set in the user object allowing us to pass it to the UI.
        /// </summary>
        /// <param name="tokenResult">Tokens for the given user (optional).</param>
        /// <returns>UserResult with the claims data and the token if it was provided.</returns>
        public UserResult GetUser(TokenResult tokenResult = null)
        {
            if (_user == null || !_user.Identity.IsAuthenticated)
                return null;

            var vendorCode = GetFirstClaimValue(ClaimConstants.VendorCodeClaimName);
            var userSession = new UserResult
            {
                FullName = GetFirstClaimValue(ClaimConstants.FullNameClaimName),
                FirstName = GetFirstClaimValue(ClaimConstants.FirstNameClaimName),
                LastName = GetFirstClaimValue(ClaimConstants.LastNameClaimName),
                Email = GetFirstClaimValue(ClaimConstants.EmailClaimName),
                MaxCreditAmount = Math.Round(Convert.ToDecimal(GetFirstClaimValue(ClaimConstants.MaxCreditApprovalClaimName)), 2),
                AdminFeesReversing = Math.Round(Convert.ToDecimal(GetFirstClaimValue(ClaimConstants.AdminFeesReversingClaimName)), 2),
                Permissions = this.RetrieveUserRoles(),
                IsPartnerUser = vendorCode != null ? true : false,
                Username = GetFirstClaimValue(ClaimConstants.UserNameClaimName),
                VendorCode = vendorCode,
                Token = tokenResult,
                Roles = this.RetrieveRoles(),
            };

            var userDbResult = GetUserResult(userSession);
            userSession.Id = userDbResult.Id;
            userSession.MaxCreditAmount = userDbResult.MaxCreditAmount ?? userSession.MaxCreditAmount;
            userSession.AdminFeesReversing = userDbResult.AdminFeesReversing ?? userSession.AdminFeesReversing;
            
            userSession.FreeMonthAllocation = userDbResult.FreeMonthAllocation;
            userSession.RetentionPrice = userDbResult.RetentionPrice;

            return userSession;
        }

        /// <summary>
        ///     Get the User DB info for a username.
        /// 
        ///     If the user does not exists we create a user profile to get a UserId.
        /// </summary>
        /// <param name="userSession">Object for the user session</param>
        /// <returns></returns>
        private UserResult GetUserResult(UserResult userSession)
        {
            var foundUser = _userService.GetUserByUserName(userSession.Username);
            if (foundUser != null) return foundUser;
            return _userService.CreateUserProfile(userSession);
        }

        private bool IsPartnerUser()
        {
            var group = this.GetFirstClaimValue(ClaimConstants.GroupClaimName);
            var isPartner = group?.ToLower().StartsWith(ClaimConstants.PartnerGroupIdentifier) ?? false;
            return isPartner;
        }

        /// <summary>
        ///     Validate a list of permission for a user.
        /// </summary>
        /// <param name="permissions">Array of permissions</param>
        /// <returns>a list of PermissionResult</returns>
        public IEnumerable<PermissionResult> AreAuthorized(PermissionType[] permissions)
        {
            foreach (var permission in permissions)
            {
                yield return new PermissionResult
                {
                    Permission = permission.ToString(),
                    Authorized = IsAuthorized(permission)
                };
            }
        }

        /// <summary>
        ///     Validate if the user has the permission
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>
        ///     <c>true</c> if the specified user is authorized; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAuthorized(PermissionType permission)
        {
            if (_user == null || !_user.Identity.IsAuthenticated)
                return false;

            if (permission == PermissionType.AllAuthenticated)
                return true;

            var isAuthorized = this.RetrieveUserRoles()?.Where(role =>
                    role.Equals(permission.ToString(), StringComparison.OrdinalIgnoreCase)
                ).Count() > 0;

            return isAuthorized;
        }

        /// <summary>
        ///     Get the first claim value found for the type requested.
        /// </summary>
        /// <param name="type">claim requested</param>
        /// <returns></returns>
        public string GetFirstClaimValue(string type)
        {
            if (_user == null)
                return null;

            var claim = _user.Claims.FirstOrDefault(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            if (claim == null)
                return null;

            return claim.Value;
        }

        /// <summary>
        ///     Get claim values
        /// </summary>
        /// <param name="type">claim requested</param>
        /// <returns></returns>
        public IEnumerable<string> GetClaimValues(string type)
        {
            if (_user == null)
                return new List<string>();

            var claims = _user.Claims
                .Where(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value);

            return claims;
        }

        /// <summary>
        /// Retrieve the roles assigned to the user for the HydroCare client. It will not retrieve realm or multi-client roles.
        /// </summary>
        /// <returns>List of roles for the user on the HydroCare client</returns>
        private IEnumerable<string> RetrieveUserRoles()
        {
            var resourceAccessJson = GetFirstClaimValue(ClaimConstants.ResourceAccessClaimName);

            if (resourceAccessJson == null)
                return null;

            var resourceAccess = JsonConvert.DeserializeObject<ResourceAccess>(resourceAccessJson);

            return resourceAccess?.Kamp?.Roles;
        }

        /// <summary>
        /// Retrieve the groups assigned to the user for the HydroCare client.
        /// </summary>
        /// <returns>List of groups for the user on the HydroCare client</returns>
        private IEnumerable<string> RetrieveRoles()
        {
            var resourceAccessJson = GetClaimValues(ClaimConstants.HydrocareRolesClaimName);

            if (resourceAccessJson == null)
                return null;

            //var resourceAccess = JsonConvert.DeserializeObject<ResourceAccess>(resourceAccessJson);

            //return resourceAccess?.HydroCare?.Roles;
            return resourceAccessJson;
        }
    }
}
