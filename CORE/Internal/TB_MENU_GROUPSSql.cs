using CORE.Base;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal
{
    internal class TB_MENU_GROUPSSql : DataAccessTable<TB_MENU_GROUPS>
    {
        public TB_MENU_GROUPSSql() : base("SERVICES.ConnectionString")
        {
        }
    }
    internal class TB_MENU_DETAILSSql : DataAccessTable<TB_MENU_DETAILS>
    {
        public TB_MENU_DETAILSSql() : base("SERVICES.ConnectionString")
        {
        }
    }
}
