using NGTDI.Library.DAOs;
using TreeGecko.Library.Mongo.Managers;
using TreeGecko.Library.Net.DAOs;

namespace NGTDI.Library.Managers
{
    public class NGTDIStructureManager : AbstractMongoManager
    {
        public NGTDIStructureManager()
            : base("NGTDI")
        {
        }

        public void BuildDB()
        {
            AntiResolutionDAO antiResolutionDAO = new AntiResolutionDAO(MongoDB);
            antiResolutionDAO.BuildTable();

            SystemEmailDAO systemEmailDAO = new SystemEmailDAO(MongoDB);
            systemEmailDAO.BuildTable();

            UserDAO userDAO = new UserDAO(MongoDB);
            userDAO.BuildTable();

            CannedEmailDAO cannedEmailDAO = new CannedEmailDAO(MongoDB);
            cannedEmailDAO.BuildTable();

            TGUserAuthorizationDAO tgUserAuthorizationDAO = new TGUserAuthorizationDAO(MongoDB);
            tgUserAuthorizationDAO.BuildTable();

            TGUserEmailValidationDAO tgUserEmailValidationDAO = new TGUserEmailValidationDAO(MongoDB);
            tgUserEmailValidationDAO.BuildTable();

            TGUserPasswordDAO tgUserPasswordDAO = new TGUserPasswordDAO(MongoDB);
            tgUserPasswordDAO.BuildTable();

            WebLogEntryDAO webLogEntryDAO = new WebLogEntryDAO(MongoDB);
            webLogEntryDAO.BuildTable();

        }
    }
}
