using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using NGTDI.Library.Managers;
using NGTDI.Library.Objects;
using Site.Objects;
using TreeGecko.Library.Common.Enums;

namespace Site.Helpers
{
    public class UserMapper : IUserMapper
    {
        public IUserIdentity GetUserFromIdentifier(Guid _identifier, NancyContext _context)
        {
            NGTDIManager manager = new NGTDIManager();

            User user = (User)manager.GetUser(_identifier);

            if (user != null)
            {
                NancyUser nUser = new NancyUser {UserName = user.Username};
                nUser.SetClaims(user.UserType);
                return nUser;
            }

            return null;
        }
    }
}