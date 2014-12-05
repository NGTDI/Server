using MongoDB.Driver;
using NGTDI.Library.Objects;
using TreeGecko.Library.Mongo.DAOs;

namespace NGTDI.Library.DAOs
{
    public class UserDAO : AbstractMongoDAO<User>
    {
        public UserDAO(MongoDatabase _mongoDB) : base(_mongoDB)
        {
            HasParent = false;
        }

        public override string TableName
        {
            get { return "Users"; }
        }

        public override void BuildTable()
        {
            base.BuildTable();

            BuildUniqueIndex("Username", "USERNAME");
            BuildUniqueSparceIndex("EmailAddress", "EMAIL");
        }

        public User Get(string _username)
        {
            return GetOneItem<User>("Username", _username);
        }

        public User GetByEmail(string _emailAddress)
        {
            return GetOneItem<User>("EmailAddress", _emailAddress);
        }
    }
}
