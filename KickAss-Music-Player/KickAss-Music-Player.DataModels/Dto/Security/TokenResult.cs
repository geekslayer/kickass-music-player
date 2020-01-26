using System;
using System.Collections.Generic;
using System.Text;

namespace KickAss_Music_Player.DataModels.Dto.Security
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public int RefreshExpiresIn { get; set; }
    }
}
