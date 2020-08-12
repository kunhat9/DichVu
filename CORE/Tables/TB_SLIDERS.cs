using CORE.Base;
using System;

namespace CORE.Tables
{
    public class TB_SLIDERS : BusinessObject
    {
        [PrimaryKey]
        public int SliderId { get; set; }
        public string SliderName { get; set; }
        public string SliderContent { get; set; }
        public DateTime SliderDateCreate { get; set; }
        public bool SliderIsShow { get; set; }
        public int SliderUserId { get; set; }

    }
}
