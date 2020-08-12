using CORE.Base;
using System;

namespace CORE.Tables
{
    public class TB_BLOGS : BusinessObject
    {
        [PrimaryKey]
        public int BlogId { get; set; }
        public string BlogName { get; set; }
        public string BlogContent { get; set; }
        public DateTime BlogDateCreate { get; set; }
        public string BlogType { get; set; }
        public bool BlogIsShow { get; set; }
        public int BlogUserId { get; set; }

    }
}
