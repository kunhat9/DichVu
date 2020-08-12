using CORE.Base;
using System;

namespace CORE.Tables
{
    public class TB_CATEGORIES : BusinessObject
    {
        [PrimaryKey]
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string CategoryState { get; set; }
        public string CategoryNote { get; set; }
    }
}
