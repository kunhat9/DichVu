using CORE.Base;
using CORE.Tables;

namespace CORE.Internal
{
    internal class TB_PRODUCTSSql : DataAccessTable<TB_PRODUCTS>
    {
        #region Constructor

        public TB_PRODUCTSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
