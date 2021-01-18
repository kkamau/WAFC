using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace APH.Admin.Models
{
    public class ActiveDirectoryValidator
    {
        private string _path;
        private string _filterAttribute;

        public ActiveDirectoryValidator(string path)
        {
            _path = path;
        }

        public bool IsAuthenticated(string domainName, string userName, string password)
        {
            string domainAndUsername = domainName + @"\" + userName;
            bool result = true;
            var credentials = new NetworkCredential(userName, password);
            var serverId = new LdapDirectoryIdentifier(domainName);

            var conn = new LdapConnection(serverId, credentials);
            try
            {
                conn.Bind();
            }
            catch (Exception ex)
            {
                //throw new Exception("Login Error: " + ex.Message);
                result = false;
            }

            conn.Dispose();

            return result;
            //DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, password);
            //try
            //{
            //    // Bind to the native AdsObject to force authentication.
            //    Object obj = entry.NativeObject;
            //    DirectorySearcher search = new DirectorySearcher(entry);
            //    search.Filter = "(SAMAccountName=" + userName + ")";
            //    search.PropertiesToLoad.Add("cn");
            //    SearchResult result = search.FindOne();
            //    if (null == result)
            //    {
            //        return false;
            //    }
            //    // Update the new path to the user in the directory
            //    _path = result.Path;
            //    _filterAttribute = (String)result.Properties["cn"][0];
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Login Error: " + ex.Message);
            //}
            //return true;
        }
    }
}