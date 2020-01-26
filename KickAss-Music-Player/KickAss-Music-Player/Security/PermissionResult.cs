namespace KickAss_Music_Player.Security
{
    /// <summary>
    /// Represent if a a user has the permission.
    /// </summary>
    public class PermissionResult
    {

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public string Permission { get; set; }
        /// <summary>
        /// Indicate if the user is authorized or not.
        /// </summary>
        public bool Authorized { get; set; }
    }
}
