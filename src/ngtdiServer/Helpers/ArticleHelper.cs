using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TreeGecko.Library.Common.Helpers;

namespace Site.Helpers
{
    public static class ArticleHelper
    {
        public static string GetUrl(Guid _arGuid)
        {
            const string URL = "{0}/publicantiresolution/{1}";
            string site = Config.GetSettingValue("SystemUrl");

            return string.Format(URL, site, _arGuid);
        }
    }
}