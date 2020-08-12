using CORE.Base;
using System;

namespace CORE.Tables
{
    public class TB_PRODUCTS : BusinessObject
    {
        [PrimaryKey]
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductState { get; set; }
        public int? ProductFileId { get; set; }
        public string ProductSupplierCode { get; set; }
        public string ProductCategoryCode { get; set; }
    }
}
