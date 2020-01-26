using KickAss_Music_Player.DataModels.Dto.Security;
using KickAss_Music_Player.DataModels.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KickAss_Music_Player.Security
{
    /// <summary>
    /// Interface for the class that return information about request token
    /// </summary>
    public interface ITokenInterpretor
    {
        /// <summary>
        /// Convert this TokenInterpretor's ClaimsPrincipal into a more usuable user object.
        /// If the token is passed, it will be set in the user object allowing us to pass it to the UI.
        /// </summary>
        /// <param name="tokenResult">Tokens for the given user (optional).</param>
        /// <returns>UserResult with the claims data and the token if it was provided.</returns>
        UserResult GetUser(TokenResult tokenResult = null);

        /// <summary>
        /// Init request user
        /// </summary>
        /// <param name="user">The user.</param>
        void InitUser(ClaimsPrincipal user);
    }
}
