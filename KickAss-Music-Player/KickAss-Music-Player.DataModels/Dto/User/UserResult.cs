using KickAss_Music_Player.DataModels.Dto.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace KickAss_Music_Player.DataModels.Dto.User
{
    public class UserResult
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public long Id { get; set; }
        public long? ClientGroupingId { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string IBToken { get; set; }
        public string Username { get; set; }
        public string VendorCode { get; set; }

        public TokenResult Token { get; set; }
        public bool IsPartnerUser { get; set; }

        public decimal? MaxCreditAmount { get; set; }
        public decimal? AdminFeesReversing { get; set; }
        public decimal? RetentionPrice { get; set; }
        public int? FreeMonthAllocation { get; set; }
        // we use this value to syncronize the time between front- and back-end sides
        public DateTime? ServerDateTime { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
