using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Tables
{
    public class TB_VOUCHERS : BusinessObject
    {
        [PrimaryKey]
        public string VoucherCode { get; set; }
        public DateTime VoucherDateCreate { get; set; }
        public DateTime VoucherDateExpired { get; set; }
        public decimal VoucherNum { get; set; }
        public string VoucherType { get; set; }
        public int VoucherLimited { get; set; }
        public string VoucherState { get; set; }
        public string VoucherNote { get; set; }
        public int VoucherServiceId { get; set; }
        public TB_VOUCHERS() { }

    }
}
