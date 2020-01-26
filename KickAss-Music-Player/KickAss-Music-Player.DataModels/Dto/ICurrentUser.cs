using KickAss_Music_Player.DataModels.Dto.User;

namespace DataModels.Dto
{
    public interface ICurrentUser
    {
        UserResult CurrentUser { get; set; }
    }
}