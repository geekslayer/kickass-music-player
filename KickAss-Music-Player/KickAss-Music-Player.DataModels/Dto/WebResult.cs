using System;
using System.Collections.Generic;
using System.Text;

namespace KickAss_Music_Player.DataModels.Dto
{
    public class WebResult
    {
        public bool Success { get; set; }
        public List<Message> Messages { get; set; }
        public object Data { get; set; }
    }
}
