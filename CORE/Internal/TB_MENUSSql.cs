using CORE.Base;
using CORE.Tables;

namespace CORE.Internal
{
    internal class TB_MENUSSql : DataAccessTable<TB_MENUS>
    {
        #region Constructor

        public TB_MENUSSql() : base("SERVICES.ConnectionString")
        {
            // Nothing for now.
        }

        #endregion
    }
}
