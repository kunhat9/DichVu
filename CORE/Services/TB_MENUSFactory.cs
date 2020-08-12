using CORE.Internal;
using CORE.Tables;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class TB_MENUSFactory
    {
        public bool Update(TB_MENUS menu)
        {
            return new TB_MENUSSql().Update(menu);
        }
        public bool Delte(int menuId)
        {
            return new TB_MENUSSql().Delete(menuId);
        }
        public List<TB_MENUS> GetAll()
        {
            return new TB_MENUSSql().SelectAll();
        }

        public TB_MENUS GetById(int menuId)
        {
            return new TB_MENUSSql().SelectByPrimaryKey(menuId);
        }
        public List<TB_MENUS> GetByServiceId(int serviceId)
        {
            return new TB_MENUSSql().FilterByField("MenuServiceId", serviceId);
        }

        public List<V_Group_Menu> GetAllDetails(int menuId)
        {
            if(menuId == 0)
            {
                return new TB_MENU_GROUPSSql().SelectAll()
                .Select(x => new V_Group_Menu
                {
                    MgroupId = x.MgroupId,
                    MgroupName = x.MgroupName,
                    MgroupSelected = x.MgroupSelected,
                    MgroupMenuId = x.MgroupMenuId,
                    MgroupDetail = new TB_MENU_DETAILSSql().FilterByField("MdetailMgroupId", x.MgroupId)
                        .Select(y => new V_Details_Menu
                        {
                            MdetailId = y.MdetailId,
                            MdetailName = y.MdetailName
                        }).ToList()
                }).ToList();
            }
            else
            {
                return new TB_MENU_GROUPSSql().FilterByField("MgroupMenuId", menuId)
                .Select(x => new V_Group_Menu
                {
                    MgroupId = x.MgroupId,
                    MgroupName = x.MgroupName,
                    MgroupSelected = x.MgroupSelected,
                    MgroupMenuId = x.MgroupMenuId,
                    MgroupDetail = new TB_MENU_DETAILSSql().FilterByField("MdetailMgroupId", x.MgroupId)
                        .Select(y => new V_Details_Menu
                        {
                            MdetailId = y.MdetailId,
                            MdetailName = y.MdetailName
                        }).ToList()
                }).ToList();
            }
            
        }

        public bool Insert(string xml)
        {
            string ecode, edesc;
            new TB_MENUSSql().SelectFromStore(out ecode, out edesc, AppSettingKeys.SP_INSERT_MENU, xml);
            return (ecode == "00");
        }
    }
}
