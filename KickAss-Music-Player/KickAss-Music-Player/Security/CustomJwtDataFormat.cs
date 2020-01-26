using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KickAss_Music_Player.Security
{
    // This class is used in XML comment below. Otherwise generates a warning.
    internal class XmlCommentCrefClass1 : ISecureDataFormat<AuthenticationTicket>
    {
        public string Protect(AuthenticationTicket data) { throw new NotImplementedException(); }
        public string Protect(AuthenticationTicket data, string purpose) { throw new NotImplementedException(); }
        public AuthenticationTicket Unprotect(string protectedText) { throw new NotImplementedException(); }
        public AuthenticationTicket Unprotect(string protectedText, string purpose) { throw new NotImplementedException(); }
    }

    /// <summary>
    /// Manage the encrypted token into a cookie.
    /// </summary>
    /// <seealso cref="XmlCommentCrefClass1" />
    public class CustomJwtDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string algorithm;
        private readonly TokenValidationParameters _validationParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomJwtDataFormat"/> class.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="validationParameters">The validation parameters.</param>
        public CustomJwtDataFormat(string algorithm, TokenValidationParameters validationParameters)
        {
            this.algorithm = algorithm;
            this._validationParameters = validationParameters;
        }

        /// <summary>
        /// Unprotects the specified protected text.
        /// </summary>
        /// <param name="protectedText">The protected text.</param>
        /// <returns></returns>
        public AuthenticationTicket Unprotect(string protectedText)
            => Unprotect(protectedText, null);

        /// <summary>
        /// Unprotects the specified protected text.
        /// </summary>
        /// <param name="protectedText">The protected text.</param>
        /// <param name="purpose">The purpose.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Invalid JWT
        /// or
        /// </exception>
        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;

            try
            {
                SecurityToken validToken;
                principal = handler.ValidateToken(protectedText, this._validationParameters, out validToken);
                var validJwt = validToken as JwtSecurityToken;

                if (validJwt == null)
                    throw new ArgumentException("Invalid JWT");

                if (!validJwt.Header.Alg.Equals(algorithm, StringComparison.Ordinal))
                    throw new ArgumentException($"Algorithm must be '{algorithm}'");
            }
            catch (SecurityTokenValidationException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            // Validation passed. Return a valid AuthenticationTicket:
            return new AuthenticationTicket(principal, new AuthenticationProperties(), "Cookie");
        }

        /// <summary>
        /// Protects the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string Protect(AuthenticationTicket data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Protects the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="purpose">The purpose.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string Protect(AuthenticationTicket data, string purpose)
        {
            throw new NotImplementedException();
        }
    }
}
