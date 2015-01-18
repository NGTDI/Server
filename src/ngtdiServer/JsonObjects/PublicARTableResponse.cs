using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NGTDI.Library.Objects;

namespace Site.JsonObjects
{
    public class PublicARTableResponse
    {
        public PublicARTableResponse()
        {
            Data = new List<PublicAntiResolution>();
        }

        public string Result { get; set; }

        public List<PublicAntiResolution> Data { get; set; } 
    }
}