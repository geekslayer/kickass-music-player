using KickAss_Music_Player.DataModels.Dto.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace KickAss_Music_Player.BusinessLogic.Services.User
{
    public class UserService : IUserService
    {
        public UserResult CreateUserProfile(UserResult userSession)
        {
            throw new NotImplementedException();
        }

        public UserResult GetUserByUserName(string userName)
        {
            return null;
        }

        public decimal? GetUserRetentionPrice(long userId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMaxCreditAmountOrAdminFeesReversingById(long id, decimal? maxCreditAmount, decimal? adminFeesReversing)
        {
            throw new NotImplementedException();
        }
    }
}
