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
    public class TB_REGISTERSFactory
    {
        public bool Insert(TB_REGISTERS register)
        {
            return new TB_REGISTERSSql().Insert(register);
        }
        public bool Update(TB_REGISTERS register)
        {
            return new TB_REGISTERSSql().Update(register);
        }
        public bool Delte(int registerId)
        {
            return new TB_REGISTERSSql().Delete(registerId);
        }
        public List<TB_REGISTERS> GetAll()
        {
            return new TB_REGISTERSSql().SelectAll();
        }

        public TB_REGISTERS GetById(int registerId)
        {
            return new TB_REGISTERSSql().SelectByPrimaryKey(registerId);
        }
        public string RegisterService(int reg_service,string reg_datebegin, string reg_number, string reg_name, string reg_email, string reg_phone, string reg_note, string reg_user, string menuId, string menu, string voucher, string reg_table)
        {
            string ecode, edesc;
            TB_REGISTERSSql sql = new TB_REGISTERSSql();
             sql.SelectFromStore(out ecode, out edesc, AppSettingKeys.SP_REGISTER, reg_service, reg_datebegin, reg_number, reg_name, reg_email, reg_phone, menuId,reg_note, reg_user, menu, voucher, reg_table);
            return ecode;
        }

        public List<V_Revenue> ReportChart(string serviceId, string menuId, string startDate, string endDate)
        {
            List<V_Revenue> list = new List<V_Revenue>();
            list = new V_RevenueSql().SelectFromStore(AppSettingKeys.SP_REPORT_CHART,serviceId,menuId,startDate,endDate);
            return list;
        }
        public List<TB_REGISTER_DETAILS> GetByRegisterId(int registerId)
        {
            return new TB_REGISTER_DETAILSSql().FilterByField("RdetailRegisterId", registerId);
        }
    }
}
