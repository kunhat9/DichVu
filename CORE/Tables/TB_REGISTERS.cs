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

        public DateTime RegisterDateCreate { get; set; }
        public DateTime RegisterDateBegin { get; set; }
        public int RegisterDateNumber { get; set; }
        public string RegisterStatus { get; set; }

        public int RegisterServiceId { get; set; }
    }
}
