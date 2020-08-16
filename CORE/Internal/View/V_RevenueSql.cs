using CORE.Base;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal.View
{
   
        internal class V_RevenueSql : DataAccessObject<V_Revenue>
        {
            public V_RevenueSql() : base("SERVICES.ConnectionString")
            { }
        }
    
}
