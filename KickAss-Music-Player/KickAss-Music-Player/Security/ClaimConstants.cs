using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickAss_Music_Player.Security
{
    /// <summary>
    /// Security constants of the application
    /// </summary>
    public class ClaimConstants
    {
        /// <summary>
        /// PermissionClaimName claim type
        /// </summary>
        public const string HydrocareRolesClaimName = "hydrocareRoles";
        /// <summary>
        /// GroupClaimName claim type
        /// </summary>
        public const string GroupClaimName = "group";
        /// <summary>
        /// EmailClaimName claim type
        /// </summary>
        public const string EmailClaimName = "email";
        /// <summary>
        /// First name claim type
        /// </summary>
        public const string FirstNameClaimName = "firstName";
        /// <summary>
        /// Lastname claim type
        /// </summary>
        public const string LastNameClaimName = "lastName";
        /// <summary>
        /// The full name
        /// </summary>
        public const string FullNameClaimName = "name";
        /// <summary>
        /// Ip address claim type
        /// </summary>
        public const string IpAddressClaimName = "clientAddress";
        /// <summary>
        /// Partner group identifier
        /// </summary>
        public const string PartnerGroupIdentifier = "/partenaires";
        /// <summary>
        /// Lastname claim type
        /// </summary>
        public const string UserNameClaimName = "preferred_username";
        /// <summary>
        /// ResourceAccess claim type
        /// </summary>
        public const string ResourceAccessClaimName = "resource_access";
        /// <summary>
        /// VendorCode claim type
        /// </summary>
        public const string VendorCodeClaimName = "vendor_code";
        /// <summary>
        /// Maximum pour crédits claim type
        /// </summary>
        public const string MaxCreditApprovalClaimName = "max_credit_approval";
        /// <summary>
        /// Renversement des frais adm. claim type
        /// </summary>
        public const string AdminFeesReversingClaimName = "admin_fees_reversing";
    }
}
