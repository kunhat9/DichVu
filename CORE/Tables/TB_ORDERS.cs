using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Tables
{
    public class TB_ORDERS : BusinessObject
    {
        [PrimaryKey]
        public string OrderCode { get; set; }

        public string OrderName { get; set; }
        public string OrderPhone { get; set; }
        public string OrderMail { get; set; }
        public string OrderAddress { get; set; }
        public string OrgerNote { get; set; }

        public string OrderNameOther { get; set; }
        public string OrderPhoneOther { get; set; }
        public string OrderMailOther { get; set; }
        public string OrderAddressOther { get; set; }

        public DateTime OrderDateCreate { get; set; }
        public decimal OrderTotal { get; set; }

        public string OrderVoucher { get; set; }
        public string OrderOption { get; set; }

        public string OrderState { get; set; }
        public int? OrderCurrentUserId { get; set; }
    }
}
