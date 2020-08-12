using CORE.Base;
using System;

namespace CORE.Tables
{
    public class TB_MENUS : BusinessObject
    {
        [PrimaryKey]
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public decimal MenuPrice { get; set; }
        public int MenuNum { get; set; }
        public string MenuNote { get; set; }
        public string MenuStatus { get; set; }
        public int MenuServiceId { get; set; }
    }
}
