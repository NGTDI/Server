using System.Collections.Generic;
using NGTDI.Library.Objects;

namespace Site.JsonObjects
{
    public class ARTableResponse
    {
        public ARTableResponse()
        {
            Data = new List<AntiResolution>();
        }
        public ARTableResponse(bool _success, 
                               User _user, 
                               List<NGTDI.Library.Objects.AntiResolution> _antiResolutions)
        {
            Data = new List<AntiResolution>();

            if (_success)
            {
                Result = "Success";
            }
            else
            {
                Result = "Failure";
            }

            foreach (NGTDI.Library.Objects.AntiResolution ar in _antiResolutions)
            {
                AntiResolution antiResolution = new AntiResolution(_user, ar);

                Data.Add(antiResolution);
            }
        }

        public string Result { get; set; }

        public List<AntiResolution> Data { get; set; } 
    }
}