using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Views
{
    public class V_Details_Service : BusinessObject
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public string ServiceUnit { get; set; }
        public string ServiceBase { get; set; }
        public string ServiceContent { get; set; }
        public string ServiceTypeCode { get; set; }
        //details
        public int SrvDetailId { get; set; }
        public string SrvDetailValue { get; set; }
        public int SrvDetailServiceId { get; set; }
        public int SrvTypeDetailId { get; set; }
    }
}
