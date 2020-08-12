using CORE.Base;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal
{
    internal class TB_VOUCHERSSql : DataAccessTable<TB_VOUCHERS>
    {
        public TB_VOUCHERSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }
    }
}
