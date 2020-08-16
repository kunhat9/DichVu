using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Tables
{
    public class TB_REGISTERS : BusinessObject
    {
        [PrimaryKey]
        public int RegisterId { get; set; }

        public string RegisterUserName { get; set; }
        public string RegisterUserEmail { get; set; }
        public string RegisterUserPhone { get; set; }
        public string RegisterUserNote { get; set; }


        public DateTime RegisterDateCreate { get; set; }
        public DateTime RegisterDateBegin { get; set; }
        public int RegisterDateNumber { get; set; }
        public string RegisterStatus { get; set; }
        public string RegisterServiceName { get; set; }
        public decimal RegisterServicePrice { get; set; }
        public string RegisterServiceUnit { get; set; }
        public string RegisterServiceBase { get; set; }
        public decimal RegisterVoucherNum { get; set; }
        public string RegisterVoucherType { get; set; }
        public int RegisterMenuNumber { get; set; }
        public decimal RegisterMenuPrice { get; set; }
        public int RegisterMenuId { get; set; }
        public int RegisterServiceId { get; set; }
        public int RegisterUserId { get; set; }



    }
}
