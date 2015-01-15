using System;
using Newtonsoft.Json;
using NGTDI.Library.Constants;
using NGTDI.Library.Objects;

namespace Site.JsonObjects
{
    public class AntiResolution
    {
        public string Guid { get; set; }
        public string Period { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Text { get; set; }
        public string EditButton { get; set; }
        public string DeleteButton { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_user"></param>
        /// <param name="_antiResolution"></param>
        /// <param name="_periodEnglish"></param>
        public AntiResolution(User _user,
            NGTDI.Library.Objects.AntiResolution _antiResolution,
            bool _periodEnglish = true)
        {
            if (_antiResolution != null)
            {
                Guid = _antiResolution.Guid.ToString();

                if (_periodEnglish)
                {
                    Period = NGTDIPeriod.ToEnglish(_antiResolution.Period);
                }
                else
                {
                    Period = _antiResolution.Period;
                }

                if (_antiResolution.StartDate != null)
                {
                    StartDate = _antiResolution.StartDate.Value.ToString("d");
                }

                if (_antiResolution.EndDate != null)
                {
                    EndDate = _antiResolution.EndDate.Value.ToString("d");
                }

                if (_user != null)
                {
                    Text = _antiResolution.GetAntiResolutionText(_user);
                }

                EditButton = string.Format("<a class=\"btn btn-info\" href=\"/editantiresolution/{0}\">Edit</a>",
                    _antiResolution.Guid);

                DeleteButton =
                    string.Format(
                        "<button class=\"btn btn-info\"onclick=\"DeleteAntiResolution('{0}');\">Delete</button>",
                        _antiResolution.Guid);
            }

        }

        [JsonIgnore]
        public bool IsAllYear
        {
            get
            {
                if (Period.Equals(NGTDIPeriod.AllYear, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }   
        }

        [JsonIgnore]
        public bool IsAfter
        {
            get
            {
                if (Period.Equals(NGTDIPeriod.After, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        [JsonIgnore]
        public bool IsBefore
        {
            get
            {
                if (Period.Equals(NGTDIPeriod.Before, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        [JsonIgnore]
        public bool IsPeriod
        {
            get
            {
                if (Period.Equals(NGTDIPeriod.Period, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        [JsonIgnore]
        public bool IsSpecificDate
        {
            get
            {
                if (Period.Equals(NGTDIPeriod.SpecificDate, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }
    }
}