using NGTDI.Library.Constants;
using NGTDI.Library.Objects;

namespace Site.JsonObjects
{
    public class PublicAntiResolution
    {
        public string Period { get; set; }
        public string Text { get; set; }

        public PublicAntiResolution(User _user,
            NGTDI.Library.Objects.AntiResolution _antiResolution,
            bool _periodEnglish = true)
        {
            if (_antiResolution != null
                && _antiResolution.IsPublic)
            {
                if (_periodEnglish)
                {
                    Period = NGTDIPeriod.ToEnglish(_antiResolution.Period);
                }
                else
                {
                    Period = _antiResolution.Period;
                }

                
                if (_user != null)
                {
                    Text = _antiResolution.GetAntiResolutionText(_user);
                }
            }
        }
    }
}