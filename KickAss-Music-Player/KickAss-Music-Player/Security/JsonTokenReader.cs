using KickAss_Music_Player.DataModels.Dto.Security;
using Newtonsoft.Json.Linq;

namespace KickAss_Music_Player.Security
{
    /// <summary>
    /// JSon Token reader
    /// </summary>
    public class JsonTokenReader
    {
        /// <summary>
        /// The id_token Json part name
        /// </summary>
        public const string IdTokenJsonPartName = "id_token";
        /// <summary>
        /// The access token json part name
        /// </summary>
        public const string AccessTokenJsonPartName = "access_token";
        /// <summary>
        /// The refresh token json part name
        /// </summary>
        public const string RefreshTokenJsonPartName = "refresh_token";
        /// <summary>
        /// The token expires in json part name
        /// </summary>
        public const string ExpiresInPartName = "expires_in";
        /// <summary>
        /// The refresh token expires in json part name
        /// </summary>
        public const string RefreshExpiresInPartName = "refresh_expires_in";
        private readonly JObject _parsedToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonTokenReader"/> class.
        /// </summary>
        /// <param name="jsonToken">The json token.</param>
        public JsonTokenReader(string jsonToken)
        {
            _parsedToken = JObject.Parse(jsonToken);
        }

        /// <summary>
        /// Gets the Id token in string.
        /// </summary>
        /// <returns></returns>
        public TokenResult GetTokenResult()
        {
            var tokenResult = new TokenResult()
            {
                AccessToken = this.GetTokenPart(AccessTokenJsonPartName),
                RefreshToken = this.GetTokenPart(RefreshTokenJsonPartName)
            };
            if (int.TryParse(GetTokenPart(ExpiresInPartName), out var intValue))
                tokenResult.ExpiresIn = intValue;

            if (int.TryParse(GetTokenPart(RefreshExpiresInPartName), out intValue))
                tokenResult.RefreshExpiresIn = intValue;

            return tokenResult;
        }

        /// <summary>
        /// Gets the token part.
        /// </summary>
        /// <param name="rawToken">The raw token.</param>
        /// <param name="partName">Name of the part.</param>
        /// <returns></returns>
        private string GetTokenPart(string partName)
        {
            return _parsedToken == null ? string.Empty : _parsedToken.Value<string>(partName);

        }
    }
}