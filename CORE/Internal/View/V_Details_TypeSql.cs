using CORE.Base;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Internal.View
{
    internal class V_Details_TypeSql : DataAccessTable<V_Details_Type>
    {
        #region Constructor

        public V_Details_TypeSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
