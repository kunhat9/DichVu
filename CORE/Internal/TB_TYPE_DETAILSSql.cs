using CORE.Base;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal
{
    internal class TB_TYPE_DETAILSSql : DataAccessTable<TB_TYPE_DETAILS>
    {
        #region Constructor

        public TB_TYPE_DETAILSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
