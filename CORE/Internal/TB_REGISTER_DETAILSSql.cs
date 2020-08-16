using CORE.Base;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal
{
    internal class TB_REGISTER_DETAILSSql : DataAccessTable<TB_REGISTER_DETAILS>
    {
        #region Constructor

        public TB_REGISTER_DETAILSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
  
}
