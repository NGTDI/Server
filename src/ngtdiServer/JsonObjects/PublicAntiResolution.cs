using System.Text;
using NGTDI.Library.Constants;
using NGTDI.Library.Objects;
using Site.Helpers;
using TreeGecko.Library.Common.Helpers;

namespace Site.JsonObjects
{
    public class PublicAntiResolution
    {
        public string Period { get; set; }
        public string Text { get; set; }
        public string FBAppID { get; set; }
        public string Url { get; set; }

        public PublicAntiResolution(User _user,
            NGTDI.Library.Objects.AntiResolution _antiResolution,
            bool _periodEnglish = true)
        {
            FBAppID = Config.GetSettingValue("FBAppID");

            if (_antiResolution != null
                && _antiResolution.IsPublic)
            {
                Url = ArticleHelper.GetUrl(_antiResolution.Guid);

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
                    StringBuilder sb = new StringBuilder(_antiResolution.GetAntiResolutionText(_user));
                    sb.Replace("\t", "");
                    Text = sb.ToString();
                }
            }
        }
    }
}