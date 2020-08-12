using CORE.Base;
using CORE.Tables;

namespace CORE.Internal
{
    internal class TB_CATEGORIESSql : DataAccessTable<TB_CATEGORIES>
    {
        #region Constructor

        public TB_CATEGORIESSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
