using CORE.Base;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal
{
    internal class TB_SLIDERSSql : DataAccessTable<TB_SLIDERS>
    {
        #region Constructor

        public TB_SLIDERSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
