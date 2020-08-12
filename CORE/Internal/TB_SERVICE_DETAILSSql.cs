using CORE.Base;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal
{
    internal class TB_SERVICE_DETAILSSql : DataAccessTable<TB_SERVICE_DETAILS>
    {
        #region Constructor

        public TB_SERVICE_DETAILSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
