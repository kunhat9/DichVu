using CORE.Base;
using CORE.Views;

namespace CORE.Internal.View
{
    internal class V_UserLoginInfo : DataAccessObject<VIEW_INFO_USER_LOGIN>
    {
        public V_UserLoginInfo() : base("SERVICES.ConnectionString")
        { }
    }
}