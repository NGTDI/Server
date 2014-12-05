using TreeGecko.Library.Common.Enums;
using TreeGecko.Library.Common.Objects;
using TreeGecko.Library.Net.Objects;

namespace NGTDI.Library.Objects
{
    public class User : TGUser
    {
        /// <summary>
        /// 
        /// </summary>
        public UserTypes UserType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TGSerializedObject GetTGSerializedObject()
        {
            TGSerializedObject obj = base.GetTGSerializedObject();

            obj.Add("UserType", UserType);

            return obj;
        }

        public override void LoadFromTGSerializedObject(TGSerializedObject _tgs)
        {
            base.LoadFromTGSerializedObject(_tgs);

            UserType = (UserTypes)_tgs.GetEnum("UserType", typeof(UserTypes), UserTypes.User);
        }
    }
}
