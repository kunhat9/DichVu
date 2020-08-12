using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Tables
{
    public class TB_SERVICES : BusinessObject
    {
        [PrimaryKey]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public string ServiceUnit { get; set; }
        public string ServiceBase { get; set; }
        public string ServiceContent { get; set; }
        public string ServiceStatus { get; set; }
        public string ServiceTypeCode { get; set; }
    }
}
