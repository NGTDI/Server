using NGTDI.Library.DAOs;
using TreeGecko.Library.Net.Managers;

namespace NGTDI.Library.Managers
{
    public class NGTDIStructureManager : AbstractCoreStructureManager
    {
        public NGTDIStructureManager()
            : base("NGTDI")
        {
        }

        public override void BuildDB()
        {
            BuildDB(false);

            AntiResolutionDAO antiResolutionDAO = new AntiResolutionDAO(MongoDB);
            antiResolutionDAO.BuildTable();

            SuggestionDAO suggestionDAO = new SuggestionDAO(MongoDB);
            suggestionDAO.BuildTable();

            UserDAO userDAO = new UserDAO(MongoDB);
            userDAO.BuildTable();
        }
    }
}
