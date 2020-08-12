using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Views
{
    public class V_Details_Type : BusinessObject
    {
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public int TypeDetailId { get; set; }
        public string TypeDetailName { get; set; }
        public string TypeDetailType { get; set; }
        public string TypeDetailTypeCode { get; set; }
    }
}
