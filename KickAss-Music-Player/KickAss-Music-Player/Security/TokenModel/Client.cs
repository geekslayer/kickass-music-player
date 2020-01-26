using Newtonsoft.Json;
using System.Collections.Generic;

namespace KickAss_Music_Player.Security.TokenModel
{
    /// <summary>
    /// Client class to represent the node in the user JWT for deserialization and ease of access
    /// </summary>
    public class Client
    {
        /// <summary>
        /// List of the roles assigned to the user for this specific client
        /// </summary>
        [JsonProperty("roles")]
        public IEnumerable<string> Roles { get; set; }
    }
}
