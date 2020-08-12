using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Views
{
    public class V_Revenue : BusinessObject
    {
        public V_Revenue() { }
        public int sMonth { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }
}
