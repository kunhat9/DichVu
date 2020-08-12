using CORE.Base;
using System;

namespace CORE.Tables
{
    public class TB_SUPPLIERS : BusinessObject
    {
        [PrimaryKey]
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierState { get; set; }
        public string SupplierNote { get; set; }
    }
}
