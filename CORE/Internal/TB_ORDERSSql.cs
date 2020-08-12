using CORE.Base;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal
{
    internal class TB_ORDERSSql : DataAccessTable<TB_ORDERS>
    {
        #region Constructor

        public TB_ORDERSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
