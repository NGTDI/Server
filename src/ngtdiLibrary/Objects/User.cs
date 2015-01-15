using TreeGecko.Library.Common.Enums;
using TreeGecko.Library.Common.Objects;
using TreeGecko.Library.Net.Objects;

namespace NGTDI.Library.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public class User : TGUser
    {
        /// <summary>
        /// 
        /// </summary>
        public UserTypes UserType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TGSerializedObject GetTGSerializedObject()
        {
            TGSerializedObject obj = base.GetTGSerializedObject();

            obj.Add("UserType", UserType);
            obj.Add("Key", Key);
            obj.Add("Salt", Salt);

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_tgs"></param>
        public override void LoadFromTGSerializedObject(TGSerializedObject _tgs)
        {
            base.LoadFromTGSerializedObject(_tgs);

            UserType = (UserTypes)_tgs.GetEnum("UserType", typeof(UserTypes), UserTypes.User);
            Key = _tgs.GetString("Key");
            Salt = _tgs.GetString("Salt");
        }
    }
}
