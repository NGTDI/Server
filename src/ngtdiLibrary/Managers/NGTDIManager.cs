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
using TreeGecko.Library.Mongo.Managers;
using TreeGecko.Library.Net.DAOs;
using TreeGecko.Library.Net.Helpers;
using TreeGecko.Library.Net.Objects;

namespace NGTDI.Library.Managers
{
    public class NGTDIManager : AbstractMongoManager
    {
        public NGTDIManager() : base("NGTDI")
        {
        }

        public AntiResolution GetAntiResolution(Guid _guid)
        {
            AntiResolutionDAO dao = new AntiResolutionDAO(MongoDB);
            return dao.Get(_guid);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_guid"></param>
        /// <returns></returns>
        public TGUserPassword GetTGUserPassword(Guid _guid)
        {
            TGUserPasswordDAO dao = new TGUserPasswordDAO(MongoDB);
            return dao.Get(_guid);
        }

        public bool ValidateUser(TGUser _user, string _testPassword)
        {
            TGUserPassword userPassword = GetTGUserPassword(_user.Guid);

            if (userPassword != null)
            {
                string testHash = TGUserPassword.HashPassword(userPassword.Salt, _testPassword);

                if (testHash.Equals(userPassword.HashedPassword))
                {
                    return true;
                }
            }

            return false;
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


        public TGUserPassword GetUserPasswordByUser(Guid _userGuid)
        {
            TGUserPasswordDAO dao = new TGUserPasswordDAO(MongoDB);
            return dao.GetOneItem<TGUserPassword>("UserGuid", _userGuid.ToString());
        }

        public void Persist(TGUserPassword _userPassword)
        {
            TGUserPasswordDAO dao = new TGUserPasswordDAO(MongoDB);
            dao.Persist(_userPassword);
        }

        public void Delete(TGUserEmailValidation _userEmailValidation)
        {
            TGUserEmailValidationDAO dao = new TGUserEmailValidationDAO(MongoDB);
            dao.Delete(_userEmailValidation);
        }

        public void Delete(AntiResolution _antiResolution)
        {
            AntiResolutionDAO dao = new AntiResolutionDAO(MongoDB);
            dao.Delete(_antiResolution);
        }

        public TGUserEmailValidation GetTGUserEmailValidation(string _emailToken)
        {
            TGUserEmailValidationDAO dao = new TGUserEmailValidationDAO(MongoDB);
            return dao.Get(_emailToken);
        }

        public void Persist(TGUserEmailValidation _emailValidation)
        {
            TGUserEmailValidationDAO dao = new TGUserEmailValidationDAO(MongoDB);
            dao.Persist(_emailValidation);
        }

        #region UserAuthorizations

        public TGUserAuthorization GetUserAuthorization(Guid _userGuid, string _authorizationToken)
        {
            TGUserAuthorizationDAO dao = new TGUserAuthorizationDAO(MongoDB);
            return dao.Get(_userGuid, _authorizationToken);
        }

        public void Persist(TGUserAuthorization _tgUserAuthorization)
        {
            TGUserAuthorizationDAO dao = new TGUserAuthorizationDAO(MongoDB);
            dao.Persist(_tgUserAuthorization);
        }

        #endregion

        #region Logging
        public void LogWarning(Guid _userGuid, string _message)
        {
            WebLogEntryDAO dao = new WebLogEntryDAO(MongoDB);

        }

        public void LogException(Guid _userGuid, Exception _message)
        {

        }

        public void LogInfo(Guid _userGuid, string _message)
        {

        }

        public void LogVerbose(Guid _userGuid, string _message)
        {

        }

        #endregion

        #region AntiResolutions

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

        #region Email
        public void Persist(CannedEmail _cannedEmail)
        {
            CannedEmailDAO dao = new CannedEmailDAO(MongoDB);
            dao.Persist(_cannedEmail);
        }

        public CannedEmail GetCannedEmail(Guid _cannedEmailGuid)
        {
            CannedEmailDAO dao = new CannedEmailDAO(MongoDB);
            return dao.Get(_cannedEmailGuid);
        }

        public CannedEmail GetCannedEmail(string _cannedEmailName)
        {
            CannedEmailDAO dao = new CannedEmailDAO(MongoDB);
            return dao.Get(_cannedEmailName);
        }

        public void Persist(SystemEmail _systemEmail)
        {
            SystemEmailDAO dao = new SystemEmailDAO(MongoDB);
            dao.Persist(_systemEmail);
        }

        public SystemEmail GetSystemEmail(Guid _guid)
        {
            SystemEmailDAO dao = new SystemEmailDAO(MongoDB);
            return dao.Get(_guid);
        }

        #endregion

#region Eula

        public TGEula GetEula(Guid _eulaGuid)
        {
            TGEulaDAO dao = new TGEulaDAO(MongoDB);
            return dao.Get(_eulaGuid);
        }

        public void Persist(TGEula _eula)
        {
            TGEulaDAO dao = new TGEulaDAO(MongoDB);
            dao.Persist(_eula);
        }

        public TGEula GetLatestEula()
        {
            TGEulaDAO dao = new TGEulaDAO(MongoDB);
            return dao.GetLatest();
        }

        public TGEulaAgreement GetEulaAgreement(Guid _userGuid, Guid _eulaGuid)
        {
            TGEulaAgreementDAO dao = new TGEulaAgreementDAO(MongoDB);
            return dao.Get(_userGuid, _eulaGuid);
        }

        public void Persist(TGEulaAgreement _eulaAgreement)
        {
            TGEulaAgreementDAO dao = new TGEulaAgreementDAO(MongoDB);
            dao.Persist(_eulaAgreement);
        }

#endregion

        #region Suggestion

        public void Persist(Suggestion _suggestion)
        {
            SuggestionDAO dao = new SuggestionDAO(MongoDB);
            dao.Persist(_suggestion);
        }

        #endregion

    }
}
