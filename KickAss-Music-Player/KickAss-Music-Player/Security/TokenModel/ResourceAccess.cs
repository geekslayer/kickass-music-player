using Newtonsoft.Json;

namespace KickAss_Music_Player.Security.TokenModel
{
    /// <summary>
    /// Resource Access class to represent the node in the user JWT for deserialization and ease of access
    /// </summary>
    public class ResourceAccess
    {
        /// <summary>
        /// In this case, we only need to retrieve the roles related of the client named "kamp"
        /// </summary>
        [JsonProperty("Kamp")]
        public Client Kamp { get; set; }
    }
}
