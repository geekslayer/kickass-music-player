using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace KickAss_Music_Player.Security
{
    /// <summary>
    /// Keycloak validation parameters
    /// </summary>
    /// <seealso cref="Microsoft.IdentityModel.Tokens.TokenValidationParameters" />
    public class KeycloakTokenValidationParameters : TokenValidationParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakTokenValidationParameters"/> class.
        /// </summary>
        /// <param name="authenticationSettings">The authentication settings.</param>
        public KeycloakTokenValidationParameters(AuthenticationSetting authenticationSettings)
        {
            var signinKey = this.GetSigningCredentials(authenticationSettings.Certificate);

            ValidateIssuerSigningKey = true;
            IssuerSigningKey = signinKey;
            IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid,
                    TokenValidationParameters validationParameters) =>
                    new List<SecurityKey> { signinKey };

            // Validate the JWT Issuer (iss) claim
            ValidateIssuer = true;
            ValidIssuer = authenticationSettings.AuthorityHost;

            // Validate the JWT Audience (aud) claim
            ValidateAudience = true;
            ValidAudience = authenticationSettings.AuthorityClientId;

            // Validate the token expiry
            ValidateLifetime = true;

            // If you want to allow a certain amount of clock drift, set that here:
            ClockSkew = TimeSpan.Zero;
        }


        private SecurityKey GetSigningCredentials(string certificateKey)
        {
            var certificate = new X509Certificate2(Convert.FromBase64String(certificateKey));
            return new X509SecurityKey(certificate);
        }
    }
}
