using System.Configuration;

namespace KickAss_Music_Player.Security
{
    /// <summary>
    /// Authentication settings (see appsetting.json)
    /// </summary>
    public class AuthenticationSetting : IAuthenticationSetting
    {

        /// <summary>
        /// Indicate to add a requirement ensure to define a PermissionClaimName or AllowAnonymous attribute on all action / controller
        /// </summary>
        public bool EnforcePresenceOfSecurityAttribute { get; set; }
        /// <summary>
        /// Gets or sets the ignored paths of security attribute.
        /// </summary>
        public string[] IgnoredPathsOfSecurityAttribute { get; set; }

        /// <summary>
        /// Force the authentication process to communicate with HTTPS.
        /// <remarks>
        /// Can be false only for dev purpose.
        /// </remarks>
        /// </summary>
        public bool? RequireHttps { get; set; }
        /// <summary>
        /// Url of the host server which is emit the token.
        /// </summary>
        public string AuthorityHost { get; set; }
        /// <summary>
        /// Client Id defined into the authority 
        /// </summary>
        public string AuthorityClientId { get; set; }
        /// <summary>
        /// Secret key used to decrypt the token
        /// </summary>
        public string AuthorityClientSecret { get; set; }
        /// <summary>
        /// Gets or sets the certificate in string value.
        /// </summary>
        public string Certificate { get; set; }

        /// <summary>
        /// Gets the Url for the rest API.
        /// ex: http://{youServer}/auth/realms/{yourRealm}/protocol/openid-connect/token
        /// </summary>
        public string OpenIdRestApiUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.AuthorityHost))
                    return string.Empty;

                var url = $"{this.AuthorityHost}/protocol/openid-connect/token";
                return url;
            }
        }

        /// <summary>
        /// Indicate if the config is correctly defined.
        /// </summary>
        /// <returns></returns>
        public void ThrowExceptionWhenInvalid()
        {
            string missingParameters = string.Empty;

            if (!this.RequireHttps.HasValue)
                missingParameters += "RequireHttps";

            if (string.IsNullOrEmpty(AuthorityHost))
                missingParameters += "AuthorityHost";

            if (string.IsNullOrEmpty(AuthorityClientId))
                missingParameters += "AuthorityClientId";

            if (string.IsNullOrEmpty(AuthorityClientSecret))
                missingParameters += "AuthorityClientSecret";

            this.ThrowMissingConfigException(missingParameters);
        }


        private void ThrowMissingConfigException(string parameterNames)
        {
            if (!string.IsNullOrEmpty(parameterNames))
                throw new ConfigurationErrorsException($"AuthenticationSetting - The parameters {parameterNames} are missing.");
        }

    }
}
