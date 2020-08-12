using CORE.Base;

namespace CORE.Tables
{
    public class TB_TYPES : BusinessObject
    {
        [PrimaryKey]
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string TypeStatus { get; set; }
        public string TypeType { get; set; }
    }
}
