using KickAss_Music_Player.DataModels.Dto.Security;
using KickAss_Music_Player.DataModels.Dto.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KickAss_Music_Player.Security
{
    /// <summary>
    /// Manage the authentication with the OpenId provider by Rest Api.
    /// </summary>
    /// <seealso cref="API.Security.IOpenIdRestApiAuthenticator" />
    public class OpenIdRestApiAuthenticator : IOpenIdRestApiAuthenticator
    {
        private readonly ITokenInterpretor _tokenInterpretor;
        private readonly IOptions<AuthenticationSetting> _authenticationSettings;
        private readonly KeycloakTokenValidationParameters _keycloakTokenValidationParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdRestApiAuthenticator"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authenticationSettings">The authentication settings.</param>
        /// <param name="keycloakTokenValidationParameters">The keycloak token validation parameters.</param>
        /// <param name="tokenInterpretor">Token information processor</param>
        /// <param name="changeLogger">log user info in bd if he has successfully authenticated</param>
        public OpenIdRestApiAuthenticator(IOptions<AuthenticationSetting> authenticationSettings, KeycloakTokenValidationParameters keycloakTokenValidationParameters,
            ITokenInterpretor tokenInterpretor)
        {
            _authenticationSettings = authenticationSettings;
            _keycloakTokenValidationParameters = keycloakTokenValidationParameters;
            _tokenInterpretor = tokenInterpretor;
        }

        /// <summary>
        /// Authenticates the user
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="loginCommand">The login command.</param>
        /// <returns></returns>
        public async Task<UserResult> AuthenticateAsync(HttpContext httpContext, LoginCommand loginCommand)
        {
            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("username", loginCommand.Username));
            formData.Add(new KeyValuePair<string, string>("password", loginCommand.Password));
            formData.Add(new KeyValuePair<string, string>("grant_type", "password"));

            var userResult = await this.AuthenticateAsync(formData);

            //if (userResult == null)
            //{
            //    LogHelper.Warn($"Invalid login attempt for user {loginCommand.Username} by {httpContext.Connection.RemoteIpAddress}");
            //}
            //else
            //{
            //    //history
            //    _changeLogger.UserLogInChangeLogger(userResult);
            //}


            return userResult;
        }

        /// <summary>
        /// Refreshes the token asynchronously.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        public async Task<UserResult> RefreshTokenAsync(HttpContext httpContext)
        {
            var refreshTokenHeader = httpContext.Request.Headers["RefreshToken"];
            if (string.IsNullOrEmpty(refreshTokenHeader))
                return null;

            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            formData.Add(new KeyValuePair<string, string>("scope", "offline_access"));
            formData.Add(new KeyValuePair<string, string>("refresh_token", refreshTokenHeader));

            return await this.AuthenticateAsync(formData);
        }

        private async Task<UserResult> AuthenticateAsync(List<KeyValuePair<string, string>> formData)
        {
            UserResult userResult = null;
            HttpClient client = null;

            //The try/catch is used to avoid to return an 500 error when the OpenId provider is not available.
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _authenticationSettings.Value.OpenIdRestApiUrl);

                client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Authorization = GetAuthenticationHeader();

                request.Content = new FormUrlEncodedContent(formData);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var jsonToken = await response.Content.ReadAsStringAsync();
                var jsonTokenReader = new JsonTokenReader(jsonToken);
                var tokenResult = jsonTokenReader.GetTokenResult();

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(tokenResult.AccessToken, _keycloakTokenValidationParameters, out SecurityToken validToken);

                _tokenInterpretor.InitUser(principal);
                userResult = _tokenInterpretor.GetUser(tokenResult);
            }
            catch
            {
                //LogHelper.Error(error);
            }
            finally
            {
                client?.Dispose();
            }

            return userResult;
        }

        private AuthenticationHeaderValue GetAuthenticationHeader()
        {
            var clientId = _authenticationSettings.Value.AuthorityClientId;
            var secret = _authenticationSettings.Value.AuthorityClientSecret;
            // ClientID:ClientSecret
            var authInfoByteArray = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
            var authorizationHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authInfoByteArray));
            return authorizationHeader;
        }


    }
}
