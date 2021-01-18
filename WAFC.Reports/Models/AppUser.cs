using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WAFC.Reports.Models
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public string Email
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }

       
    }
}