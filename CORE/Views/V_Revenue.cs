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
        public DateTime DateCreated { get; set; }
        public decimal MenuPrice { get; set; }
        public decimal ServicePrice { get; set; }
    }
}
