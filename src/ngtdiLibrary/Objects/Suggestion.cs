using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeGecko.Library.Common.Objects;

namespace NGTDI.Library.Objects
{
    public class Suggestion : AbstractTGObject
    {
        public Guid SubmittedByUserGuid { get; set; }
        public string Text { get; set; }

        public override TGSerializedObject GetTGSerializedObject()
        {
            TGSerializedObject tgs = base.GetTGSerializedObject();

            tgs.Add("SubmittedByUserGuid", SubmittedByUserGuid);
            tgs.Add("Text", Text);

            return tgs;
        }

        public override void LoadFromTGSerializedObject(TGSerializedObject _tgs)
        {
            base.LoadFromTGSerializedObject(_tgs);

            SubmittedByUserGuid = _tgs.GetGuid("SubmittedByUserGuid");
            Text = _tgs.GetString("Text");
        }
    }
}
