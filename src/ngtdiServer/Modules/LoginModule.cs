using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Authentication.Forms;
using NGTDI.Library.Constants;
using NGTDI.Library.Managers;
using NGTDI.Library.Objects;
using Site.Helpers;
using TreeGecko.Library.Common.Enums;
using TreeGecko.Library.Common.Helpers;
using TreeGecko.Library.Common.Security;
using TreeGecko.Library.Net.Objects;

namespace Site.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule()
        {
            Get["/login"] = _parameters =>
            {
                return View["login.sshtml"];
            };

            Post["/login"] = _parameters =>
            {
                User user;

                string username = Request.Headers["Username"].First();
                string password = Request.Headers["Password"].First();

                string result = AuthHelper.Authorize(username, password, out user);

                Response response;

                if (user != null)
                {
                    response = this.LoginWithoutRedirect(user.Guid, DateTime.UtcNow.AddDays(30));
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(result);

                    response.Contents = _stream => _stream.Write(jsonBytes, 0, jsonBytes.Length); 
                }
                else
                {
                    response = result;
                }
                 
                response.ContentType = "application/json";
                return response;
            };

            Get["/logout"] = _parameters =>
            {
                return this.LogoutAndRedirect("/logoutcomplete");
            };

            Get["/logoutcomplete"] = _parameters =>
            {
                return View["logout.sshtml"];
            };

            Get["/reset"]  = _parameters =>
            {
                return View["reset.sshtml"];
            };

            Post["/reset"] = _parameters =>
            {
                Response response= HandleResetPassword(_parameters);
                response.ContentType = "application/json";
                return response;
            };

            Get["/register"] = _parameters =>
            {
                return View["register.sshtml"];
            };

            Post["/register"] = _parameters =>
            {
                Response response = (Response)Register(_parameters);
                response.ContentType = "application/json";
                return response;
            };

            Get["/emailvalidation/{validationtoken}"] = _parameters =>
            {
                bool result = HandleEmailValidation(_parameters);

                if (result)
                {
                    return View["emailvalidationsuccess.sshtml"];
                }

                return View["emailvalidationfailure.sshtml"];
            };
        }

        private bool HandleEmailValidation(DynamicDictionary _parameters)
        {
            string validationToken = _parameters["validationtoken"];

            if (!string.IsNullOrEmpty(validationToken))
            {
                NGTDIManager manager = new NGTDIManager();

                TGUserEmailValidation uev = manager.GetTGUserEmailValidation(validationToken);

                if (uev != null
                    && uev.ParentGuid != null)
                {
                    User user = (User)manager.GetUser(uev.ParentGuid.Value);

                    if (user != null)
                    {
                        user.IsVerified = true;

                        manager.Persist(user);
                        manager.Delete(uev);

                        return true;
                    }
                    else
                    {
                        //User not found.
                    }
                }
                else
                {
                    //Validation text not found in database
                }
            }
            else
            {
                //Validation text not supplied.
            }

            return false;
        }

        private string Register(DynamicDictionary _parameters)
        {
            string username = Request.Headers["Username"].First();
            string email = Request.Headers["Email"].First();
            string password = Request.Headers["Password"].First();

            NGTDIManager manager = new NGTDIManager();

            User user = manager.GetUser(username);

            if (user == null)
            {
                user = new User
                {
                    IsVerified = false,
                    Active = true,
                    DisplayName = username,
                    EmailAddress = email,
                    UserType = UserTypes.User,
                    Username = username
                };
                manager.Persist(user);

                TGUserPassword userPassword = TGUserPassword.GetNew(user.Guid, username, password);
                manager.Persist(userPassword);

                TGUserEmailValidation validation = new TGUserEmailValidation(user);
                manager.Persist(validation);

                NameValueCollection nvc = new NameValueCollection
                {
                    {"SystemUrl", Config.GetSettingValue("SystemUrl")},
                    {"ValidationText", validation.ValidationText }
                };
                manager.SendCannedEmail(user, CannedEmailNames.ValidateEmailAddress, nvc);

                return "{ \"Result\":\"Success\" }";
            }

            return "{ \"Result\":\"UsernameNotAvailable\" }";
        }


        private string HandleResetPassword(DynamicDictionary _parameters)
        {
            string email = Request.Headers["EmailAddress"].First();

            if (email != null)
            {
                NGTDIManager manager = new NGTDIManager();
                User user = manager.GetUserByEmail(email.ToLower());

                if (user != null)
                {
                    string newPassword = RandomString.GetRandomString(10);

                    TGUserPassword up = TGUserPassword.GetNew(user.Guid, user.Username, newPassword);
                    manager.Persist(up);

                    NameValueCollection nvc = new NameValueCollection
                    {
                        {"Password", newPassword}
                    };

                    manager.SendCannedEmail(user, CannedEmailNames.ResetPasswordEmail, nvc);
                }

                return "{ \"Result\":\"Success\"}";
            }

            return "{ \"Result\":\"Failure\"}";
        }
    }
}