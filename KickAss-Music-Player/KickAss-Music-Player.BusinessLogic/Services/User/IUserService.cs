using KickAss_Music_Player.DataModels.Dto.User;

namespace KickAss_Music_Player.BusinessLogic.Services.User
{
    public interface IUserService
    {
        UserResult GetUserByUserName(string userName);
        bool UpdateMaxCreditAmountOrAdminFeesReversingById(long id, decimal? maxCreditAmount,
            decimal? adminFeesReversing);
        UserResult CreateUserProfile(UserResult userSession);
        //UserResult EditUserParameters(UserEditParametersCommand command);
        decimal? GetUserRetentionPrice(long userId);
    }
}
