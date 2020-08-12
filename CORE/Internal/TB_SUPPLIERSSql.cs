using CORE.Base;
using CORE.Tables;

namespace CORE.Internal
{
    internal class TB_SUPPLIERSSql : DataAccessTable<TB_SUPPLIERS>
    {
        #region Constructor

        public TB_SUPPLIERSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
