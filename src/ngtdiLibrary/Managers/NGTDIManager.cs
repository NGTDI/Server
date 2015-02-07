using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using NGTDI.Library.DAOs;
using NGTDI.Library.Objects;
using TreeGecko.Library.AWS.Helpers;
using TreeGecko.Library.Common.Helpers;
using TreeGecko.Library.Common.Objects;
using TreeGecko.Library.Common.Security;
using TreeGecko.Library.Net.Helpers;
using TreeGecko.Library.Net.Managers;
using TreeGecko.Library.Net.Objects;

namespace NGTDI.Library.Managers
{
    public class NGTDIManager : AbstractCoreManager
    {
        public NGTDIManager() : base("NGTDI")
        {
        }

        #region Users
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_user"></param>
        public void Persist(User _user)
        {
            UserDAO dao = new UserDAO(MongoDB);
            dao.Persist(_user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_userGuid"></param>
        /// <returns></returns>
        public TGUser GetUser(Guid _userGuid)
        {
            UserDAO dao = new UserDAO(MongoDB);
            User user = dao.Get(_userGuid);

            if (string.IsNullOrEmpty(user.Key))
            {
                string password = RandomString.GetRandomString(16);
                byte[] temp = Encoding.ASCII.GetBytes(password);

                user.Key = Convert.ToBase64String(temp);
                user.Salt = TGUserPassword.GenerateSalt(user.Key, 16);

                dao.Persist(user);
            }

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_username"></param>
        /// <returns></returns>
        public User GetUser(string _username)
        {
            UserDAO dao = new UserDAO(MongoDB);
            return dao.Get(_username);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            UserDAO dao = new UserDAO(MongoDB);
            return dao.GetAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_emailAddress"></param>
        /// <returns></returns>
        public User GetUserByEmail(string _emailAddress)
        {
            UserDAO dao = new UserDAO(MongoDB);
            return dao.GetByEmail(_emailAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long GetUserCount()
        {
            return 0;
        }

        #endregion

        #region AntiResolutions

        public AntiResolution GetAntiResolution(Guid _guid)
        {
            AntiResolutionDAO dao = new AntiResolutionDAO(MongoDB);
            return dao.Get(_guid);
        }

        public void Delete(AntiResolution _antiResolution)
        {
            AntiResolutionDAO dao = new AntiResolutionDAO(MongoDB);
            dao.Delete(_antiResolution);
        }

        public void Persist(AntiResolution _antiResolution)
        {
            AntiResolutionDAO dao = new AntiResolutionDAO(MongoDB);
            dao.Persist(_antiResolution);
        }

        public List<AntiResolution> GetAntiResolutions(Guid _userGuid)
        {
            AntiResolutionDAO dao = new AntiResolutionDAO(MongoDB);
            return dao.GetAntiResolutions(_userGuid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_userGuid"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public List<AntiResolution> GetAntiResolutions(Guid _userGuid,
            DateTime _startDate, DateTime _endDate)
        {
            return null;
        }

        public List<AntiResolution> GetPublicAntiResolutions(int _limit)
        {
            AntiResolutionDAO dao = new AntiResolutionDAO(MongoDB);
            return dao.GetPublicAntiResolutions(_limit);
        }

        #endregion

        public bool SendCannedEmail(TGUser _tgUser,
                                    string _cannedEmailName,
                                    NameValueCollection _additionParameters)
        {
            try
            {
                CannedEmail cannedEmail = GetCannedEmail(_cannedEmailName);

                if (cannedEmail != null)
                {
                    SystemEmail email = new SystemEmail(cannedEmail.Guid);

                    TGSerializedObject tgso = _tgUser.GetTGSerializedObject();
                    foreach (string key in _additionParameters.Keys)
                    {
                        string value = _additionParameters.Get(key);
                        tgso.Add(key, value);
                    }

                    CannedEmailHelper.PopulateEmail(cannedEmail, email, tgso);

                    SESHelper.SendMessage(email);
                    Persist(email);

                    return true;
                }

                TraceFileHelper.Warning("Canned email not found");
            }
            catch (Exception ex)
            {
                TraceFileHelper.Exception(ex);
            }

            return false;
        }


        #region Suggestion

        public void Persist(Suggestion _suggestion)
        {
            SuggestionDAO dao = new SuggestionDAO(MongoDB);
            dao.Persist(_suggestion);
        }

        #endregion

    }
}
