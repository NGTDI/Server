using System;
using NGTDI.Library.Managers;
using TreeGecko.Library.Common.Enums;
using TreeGecko.Library.Common.Helpers;
using TreeGecko.Library.Net.Enums;
using TreeGecko.Library.Net.Objects;

namespace NGTDI.Console
{
    class Program
    {
        static void Main(string[] _args)
        {
            TraceFileHelper.SetupLogging();

            if (_args != null
                && _args.Length > 0)
            {
                string action = _args[0].ToLower();

                switch (action)
                {
                    case "builddb":
                        BuildDB();
                        break;
                    case "adduser":
                        if (_args.Length >= 4)
                        {
                            AddUser(_args[1], _args[2], _args[3]);
                        }
                        break;
                    case "buildcannedemail":
                        BuildEmail();
                        break;
                    default:
                        System.Console.WriteLine("No action requested");
                        break;
                }
            }

            TraceFileHelper.TearDownLogging(); 
        }

        private static void BuildEmail()
        {
            NGTDIManager manager = new NGTDIManager();

            CannedEmail resetPasswordEmail = new CannedEmail
            {
                Active = true,
                BodyType = EmailBodyType.HTML,
                Description = "Sent when a user needs to have their password reset",
                From = "noreply@ngtdi.com",
                Guid = new Guid("cab0206c-0f96-420e-b384-cb005d90af48"),
                Name = "Reset Password Email",
                ReplyTo = "noreply@ngtdi.com",
                Subject = "NGTDI Password Reset",
                To = "[[EmailAddress]]",
                Body =
                    "<p>We have recieved a request to reset your password to the NGTDI system.</p><p>Your username is [[Username]].</p><p>Your password has been changed to [[Password]].</p>"
            };
            manager.Persist(resetPasswordEmail);

            CannedEmail emailAddressValidateEmail = new CannedEmail
            {
                Active = true,
                BodyType = EmailBodyType.HTML,
                Description = "Sent when a user needs to verify their email address",
                From = "noreply@ngtdi.com",
                Guid = new Guid("b3bf887e-82e7-4662-9335-373f8827374b"),
                Name = "Validate Email Address",
                ReplyTo = "noreply@ngtdi.com",
                Subject = "NGTDI Email Validation",
                To = "[[EmailAddress]]",
                Body =
                    "<p>Please click the following link to complete your setup as a NGTDI user. <a href=\"[[SystemUrl]]/emailvalidation/[[ValidationText]]\">Validate my Email</a></p>"
            };
            manager.Persist(emailAddressValidateEmail);
        }

        private static void AddUser(string _username, string _email, string _password)
        {
            NGTDIManager manager = new NGTDIManager();

            Library.Objects.User user = new Library.Objects.User
            {
                UserType = UserTypes.User,
                Username = _username,
                EmailAddress = _email,
                IsVerified = true
            };
            manager.Persist(user);

            TGUserPassword userPassword = TGUserPassword.GetNew(user.Guid, _username, _password);
            manager.Persist(userPassword);
        }

        private static void BuildDB()
        {
            NGTDIStructureManager manager = new NGTDIStructureManager();
            manager.BuildDB();
        }
    }
}
