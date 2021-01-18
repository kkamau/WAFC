using WAFC.Reports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WAFC.Reports.org.cgiar.ocs.icraf;
using System.Configuration;

namespace WAFC.Reports.Controllers
{
    public class LayoutController : Controller
    {
        [ChildActionOnly]
        public PartialViewResult SideBarUserArea()
        {
            string email = User.Identity.Name;// ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email).Value;
            string financialyear = string.Empty;
            string calendar = string.Empty;
            

            SideBarUserAreaViewModel userVM = new SideBarUserAreaViewModel
            {
               // Photo = User.Identity.GetUserProfilePhoto(),
                UserNames = User.Identity.Name,
                EmailAddress = User.Identity.Name,
  
            };

            if (Session["speriod"] == null)
            {
                Session["speriod"] = DateTime.Now.Year.ToString() + "01";
                Session["eperiod"] = DateTime.Now.Year.ToString() + "12";
                Session["syear"] = DateTime.Now.Year.ToString();
            }
            if (Session["WSCredentials"] == null)
            {
                Session["WSCredentials"] = GetUserCredentials();

            }
            return PartialView("~/Views/Shared/DisplayTemplates/_SideBarUserArea.cshtml", userVM);
        }

        public AppUser CurrentUser
        {
            get
            {
                return new AppUser(this.User as ClaimsPrincipal);
            }
        }
        public WSCredentials GetUserCredentials()
        {
            WSCredentials cred = new WSCredentials
            {
                Username = ConfigurationManager.AppSettings["aUsername"],
                Password = ConfigurationManager.AppSettings["aPassword"],
                Client = ConfigurationManager.AppSettings["aClient"]
            };
            return cred;
        }
        public string GetCurrentUserEmail()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var email = claims.Where(c => c.Type == ClaimTypes.Email).ToList();
            return email[0].Value.ToString();
        }
    }
}