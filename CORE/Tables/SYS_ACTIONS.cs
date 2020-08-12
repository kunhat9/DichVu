using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Tables
{
    public class SYS_ACTIONS
    {
        #region Properties

        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsModule { get; set; }

        public bool IsRoot { get; set; }

        public bool IsShow { get; set; }

        public int OrderX { get; set; }

        public string ControlPath { get; set; }
        public string ActDes { get; set; }
        public string IconClass { get; set; }
        public string ParentId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAdmin { get; set; }

        #endregion
    }
}
