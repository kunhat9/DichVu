using CORE.Internal;
using CORE.Internal.View;
using CORE.Tables;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class TB_USERSFactory
    {
        public bool Insert(TB_USERS user)
        {
            return new TB_USERSSql().Insert(user);
        }

        public bool Update(TB_USERS user)
        {
            return new TB_USERSSql().Update(user);
        }
        public List<TB_USERS> GetAll()
        {
            return new TB_USERSSql().SelectAll();
        }
        public TB_USERS GetById(int userId)
        {
            return new TB_USERSSql().SelectByPrimaryKey(userId);
        }

        public VIEW_INFO_USER_LOGIN CheckLogin(string username, string password)
        {
            List<VIEW_INFO_USER_LOGIN> result;

            try
            {
                result = new V_UserLoginInfo().SelectFromStore(AppSettingKeys.SP_CHECK_LOGIN, username, password);
                if (result.Count == 1)
                {
                    return result[0];
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
