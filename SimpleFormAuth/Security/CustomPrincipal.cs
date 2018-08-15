using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SimpleFormAuth.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string UserRole)
        {
            if (Role == UserRole)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
