using System;
using TreeGecko.Library.Common.Objects;

namespace NGTDI.Library.Objects
{
    public class AntiResolution : AbstractTGObject
    {
        public string Period { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Text { get; set; }

        public override TGSerializedObject GetTGSerializedObject()
        {
            TGSerializedObject tgs = base.GetTGSerializedObject();

            tgs.Add("Period", Period);
            tgs.Add("StartDate", StartDate);
            tgs.Add("EndDate", EndDate);
            tgs.Add("Text", Text);

            return tgs;
        }

        public override void LoadFromTGSerializedObject(TGSerializedObject _tgs)
        {
            base.LoadFromTGSerializedObject(_tgs);

            Period = _tgs.GetString("Period");
            StartDate = _tgs.GetNullableDateTime("StartDate");
            EndDate = _tgs.GetNullableDateTime("EndDate");
            Text = _tgs.GetString("Text");
        }
    }
}
