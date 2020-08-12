using CORE.Base;

namespace CORE.Tables
{
    public class TB_MENU_GROUPS : BusinessObject
    {
        [PrimaryKey]
        public int MgroupId { get; set; }
        public string MgroupName { get; set; }
        public int MgroupSelected { get; set; } = 1;
        public int MgroupMenuId { get; set; }
    }

    public class TB_MENU_DETAILS : BusinessObject
    {
        [PrimaryKey]
        public int MdetailId { get; set; }
        public string MdetailName { get; set; }
        public int MdetailMgroupId { get; set; }
    }
}
