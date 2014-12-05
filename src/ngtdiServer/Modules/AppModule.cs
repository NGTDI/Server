using System.Linq;
using Nancy;
using Nancy.Security;
using NGTDI.Library.Managers;
using NGTDI.Library.Objects;
using Site.Helpers;
using TreeGecko.Library.Net.Objects;

namespace Site.Modules
{
    public class AppModule : NancyModule
    {
        public AppModule()
        {
            this.RequiresAuthentication();

            Get["/"] = _parameters =>
            {
                return View["default.sshtml"];
            };

            Get["/home"] = _parameters =>
            {
                return View["home.sshtml"];
            };

            Get["/addantiresolution"] = _parameters =>
            {
                return View["addantiresolution.sshtml"];
            };

            Get["/myantiresolutions"] = _parameters =>
            {
                return View["myantiresolutions.sshtml"];
            };

            Get["/changepassword"] = _parameters =>
            {
                return View["changepassword.sshtml"];
            };

            Post["/changepassword"] = _parameters =>
            {
                Response response = HandleChangePassword(_parameters);
                response.ContentType = "application/json";
                return response;
            };
        }

        private string HandleChangePassword(DynamicDictionary _parameters)
        {
            string currentPassword = Request.Form["currentpassword"];
            string newPassword = Request.Form["newpassword"];
            string username = Request.Headers["Username"].First();

            User user = null;
            AuthHelper.Authorize(username, currentPassword, out user);

            if (user != null)
            {
                TGUserPassword up = TGUserPassword.GetNew(user.Guid, user.Username, newPassword);

                NGTDIManager manager = new NGTDIManager();
                manager.Persist(up);

                return @"{ ""Result"":""Success"" }"; ;
            }

            return @"{ ""Result"":""Failure"" }";
        }
    }
}