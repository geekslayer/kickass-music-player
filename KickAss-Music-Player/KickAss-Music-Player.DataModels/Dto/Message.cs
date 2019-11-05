namespace KickAss_Music_Player.DataModels.Dto
{
    public enum WebMessageType
    {
        Error,
        Warning,
        Info,
        Success
    }

    public class Message
    {
        public WebMessageType Type { get; set; }
        public string MessageText { get; set; }
    }
}