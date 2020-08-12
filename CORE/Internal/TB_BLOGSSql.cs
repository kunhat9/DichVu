using CORE.Base;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal
{
    internal class TB_BLOGSSql : DataAccessTable<TB_BLOGS>
    {
        #region Constructor

        public TB_BLOGSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
