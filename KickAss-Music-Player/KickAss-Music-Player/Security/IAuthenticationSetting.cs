namespace KickAss_Music_Player.Security
{
    public interface IAuthenticationSetting
    {
        /// <summary>
        /// Indicate to add a requirement ensure to define a PermissionClaimName or AllowAnonymous attribute on all action / controller
        /// </summary>
        bool EnforcePresenceOfSecurityAttribute { get; set; }

        /// <summary>
        /// Force the authentication process to communicate with HTTPS.
        /// <remarks>
        /// Can be false only for dev purpose.
        /// </remarks>
        /// </summary>
        bool? RequireHttps { get; set; }

        /// <summary>
        /// Url of the host server which is emit the token.
        /// </summary>
        string AuthorityHost { get; set; }

        /// <summary>
        /// Client Id defined into the authority 
        /// </summary>
        string AuthorityClientId { get; set; }

        /// <summary>
        /// Secret key used to decrypt the token
        /// </summary>
        string AuthorityClientSecret { get; set; }

        /// <summary>
        /// Gets the Url for the rest API.
        /// ex: http://{youServer}/auth/realms/{yourRealm}/protocol/openid-connect/token
        /// </summary>
        string OpenIdRestApiUrl { get; }

        /// <summary>
        /// Gets or sets the ignored paths of security attribute.
        /// </summary>
        string[] IgnoredPathsOfSecurityAttribute { get; set; }
    }
}
