using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace WAFC.Reports.Models
{
    public static class IdentityExtensions
    {
        public static string GetUserProfilePhoto(this IIdentity identity)
        {
            var ProfilePhoto = ((ClaimsIdentity)identity).FindFirst("Photo");
            return (ProfilePhoto != null) ? ProfilePhoto.Value : string.Empty;
        }
        public static string GetUserNames(this IIdentity identity)
        {
            var FirstName = ((ClaimsIdentity)identity).FindFirst("FirstName");
            var LastName = ((ClaimsIdentity)identity).FindFirst("LastName");
            // return (FirstName != null) ? FirstName.Value : string.Empty;
            return FirstName.Value + " " + LastName.Value;
        }
        public static string GetEmailAddress(this IIdentity identity)
        {
            var EmailAddress = ((ClaimsIdentity)identity).FindFirst("Email");
            return (EmailAddress != null) ? EmailAddress.Value : string.Empty;

        }
    }
}