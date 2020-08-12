using CORE.Internal;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class TB_ORDERSFactory
    {
        public bool Insert(TB_ORDERS order)
        {
            return new TB_ORDERSSql().Insert(order);
        }
        public bool Update(TB_ORDERS order)
        {
            return new TB_ORDERSSql().Update(order);
        }
        public bool Delte(string orderCode)
        {
            return new TB_ORDERSSql().Delete(orderCode);
        }
        public List<TB_ORDERS> GetAll()
        {
            return new TB_ORDERSSql().SelectAll();
        }

        public TB_ORDERS GetById(string orderCode)
        {
            return new TB_ORDERSSql().SelectByPrimaryKey(orderCode);
        }

        public bool InsertOrder(string xmlOrder, string xmlDetail, out string edesc)
        {
            string ecode = ""; edesc = "";
            //new TB_ORDERSSql().SelectFromStore(out ecode, out edesc, AppSettingKeys.SP_ORDER_INSERT, xmlOrder, xmlDetail);
            return (ecode == "00");
        }
    }
}
