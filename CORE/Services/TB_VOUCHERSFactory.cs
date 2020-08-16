using CORE.Internal;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class TB_VOUCHERSFactory
    {
        public bool Insert(TB_VOUCHERS voucher)
        {
            return new TB_VOUCHERSSql().Insert(voucher, true);
        }
        public bool Update(TB_VOUCHERS voucher)
        {
            return new TB_VOUCHERSSql().Update(voucher);
        }
        public TB_VOUCHERS GetById(string voucherCode)
        {
            return new TB_VOUCHERSSql().SelectByPrimaryKey(voucherCode);
        }
        public List<TB_VOUCHERS> GetAll()
        {
            return new TB_VOUCHERSSql().SelectAll();
        }

        public TB_VOUCHERS GetByCode(string code)
        {
            return new TB_VOUCHERSSql().SelectAll().Where(x => x.VoucherCode == code).FirstOrDefault();
        }
    }
}
