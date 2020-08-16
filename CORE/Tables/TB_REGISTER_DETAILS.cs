using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Tables
{
    public class TB_REGISTER_DETAILS : BusinessObject
    {
        [PrimaryKey]
        public int RdetailId { get; set; }
        public int RdetailMdetailId { get; set; }
        public int RdetailRegisterId { get; set; }

    }
}
