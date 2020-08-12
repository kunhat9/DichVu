using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Tables
{
    public class TB_TYPE_DETAILS : BusinessObject
    {
        public TB_TYPE_DETAILS() { }
        public enum TB_TYPE_DETAILS_Field
        {
            TypeDetailId,
            TypeDetailName,
            TypeDetailType,
            TypeDetailTypeCode

        }
        [PrimaryKey]
        public int TypeDetailId { get; set; }
        public string TypeDetailName { get; set; }
        public string TypeDetailType { get; set; }
        public string TypeDetailTypeCode { get; set; }

    }
}
