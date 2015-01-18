using System;
using System.Security.Cryptography;
using System.Text;
using BCN.Library.Security;
using TreeGecko.Library.Common.Objects;
using TreeGecko.Library.Common.Security;

namespace NGTDI.Library.Objects
{
    public class AntiResolution : AbstractTGObject
    {
        public string Period { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Text { get; set; }
        public bool IsPublic { get; set; } 

        public override TGSerializedObject GetTGSerializedObject()
        {
            TGSerializedObject tgs = base.GetTGSerializedObject();

            tgs.Add("Period", Period);
            tgs.Add("StartDate", StartDate);
            tgs.Add("EndDate", EndDate);
            tgs.Add("Text", Text);
            tgs.Add("IsPublic", IsPublic);

            return tgs;
        }

        public override void LoadFromTGSerializedObject(TGSerializedObject _tgs)
        {
            base.LoadFromTGSerializedObject(_tgs);

            Period = _tgs.GetString("Period");
            StartDate = _tgs.GetNullableDateTime("StartDate");
            EndDate = _tgs.GetNullableDateTime("EndDate");
            Text = _tgs.GetString("Text");
            IsPublic = _tgs.GetBoolean("IsPublic");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_user"></param>
        /// <param name="_antiresolution"></param>
        public void SetAntiResolutionText(User _user, string _antiresolution)
        {
            CryptoString cs = new CryptoString(_user.Key, _user.Salt);

            Text = cs.Encrypt(_antiresolution);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_user"></param>
        /// <returns></returns>
        public string GetAntiResolutionText(User _user)
        {
            CryptoString cs = new CryptoString(_user.Key, _user.Salt);

            return cs.Decrypt(Text);
        }
    }
}
