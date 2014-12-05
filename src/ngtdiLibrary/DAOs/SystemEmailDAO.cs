using MongoDB.Driver;
using NGTDI.Library.Objects;
using TreeGecko.Library.Mongo.DAOs;

namespace NGTDI.Library.DAOs
{
    public class SystemEmailDAO : AbstractMongoDAO<SystemEmail>
    {
        public SystemEmailDAO(MongoDatabase _mongoDB)
            : base(_mongoDB)
        {
            HasParent = false;
        }

        public override string TableName
        {
            get { return "SystemEmail"; }
        }

        public override void BuildTable()
        {
            base.BuildTable();

            BuildNonuniqueIndex("SentDateTime", "SENT");
        }
    }
}
