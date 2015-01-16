using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NGTDI.Library.Objects;
using TreeGecko.Library.Mongo.DAOs;

namespace NGTDI.Library.DAOs
{
    internal class SuggestionDAO: AbstractMongoDAO<Suggestion>
    {
        public SuggestionDAO(MongoDatabase _mongoDB)
            : base(_mongoDB)
        {
            //
            HasParent = false;
        }

        public override string TableName
        {
            get { return "Suggestions"; }
        }
    }
}
