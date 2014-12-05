using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NGTDI.Library.Objects;
using TreeGecko.Library.Mongo.DAOs;

namespace NGTDI.Library.DAOs
{
    public class AntiResolutionDAO : AbstractMongoDAO<AntiResolution>
    {
        public AntiResolutionDAO(MongoDatabase _mongoDB) : base(_mongoDB)
        {
            //
            HasParent = true;
        }

        public override string TableName
        {
            get { return "AntiResolutions"; }
        }

        public List<AntiResolution> GetAntiResolutions(Guid _userGuid)
        {
            IMongoQuery query = GetQuery("ParentGuid", _userGuid.ToString());
            return GetList(GetCursor(query).SetSortOrder(SortBy.Ascending("StartDate")));
        }
    }
}
