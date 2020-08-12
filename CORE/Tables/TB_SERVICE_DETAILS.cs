using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Tables
{
    public class TB_SERVICE_DETAILS : BusinessObject
    {
        [PrimaryKey]
        public int SrvDetailId { get; set; }
        public string SrvDetailValue { get; set; }
        public int SrvDetailServiceId { get; set; }
        public int SrvTypeDetailId { get; set; }

    }
}
