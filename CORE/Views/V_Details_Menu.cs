using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Views
{
    public class V_Details_Menu
    {
        public int MdetailId { get; set; }
        public string MdetailName { get; set; }
    }

    public class V_Group_Menu
    {
        public int MgroupId { get; set; }
        public string MgroupName { get; set; }
        public int MgroupSelected { get; set; } = 1;
        public int MgroupMenuId { get; set; }

        public List<V_Details_Menu> MgroupDetail { get; set; }
    }
}
