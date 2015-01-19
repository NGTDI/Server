using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            //User
            HasParent = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string TableName
        {
            get { return "AntiResolutions"; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_userGuid"></param>
        /// <returns></returns>
        public List<AntiResolution> GetAntiResolutions(Guid _userGuid)
        {
            IMongoQuery query = GetQuery("ParentGuid", _userGuid.ToString());
            return GetList(GetCursor(query).SetSortOrder(SortBy.Ascending("StartDate")));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_limit"></param>
        /// <returns></returns>
        public List<AntiResolution> GetPublicAntiResolutions(int _limit)
        {
            NameValueCollection nvc = new NameValueCollection {{"IsPublic", "True"}, {"IsFlagged", "False"}};

            IMongoQuery query = GetQuery(nvc);

            MongoCursor cursor = GetCursor(query)
                .SetSortOrder(SortBy.Ascending("LastModifiedDateTime"))
                .SetLimit(_limit);

            return GetList(cursor);
        }
    }
}
