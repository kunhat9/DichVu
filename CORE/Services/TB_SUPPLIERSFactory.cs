using CORE.Internal;
using CORE.Tables;
using System;
using System.Collections.Generic;

namespace CORE.Services
{
    public class TB_SUPPLIERSFactory
    {
        public bool Insert(TB_SUPPLIERS supplier)
        {
            return new TB_SUPPLIERSSql().Insert(supplier, true);
        }
        public bool Update(TB_SUPPLIERS supplier)
        {
            return new TB_SUPPLIERSSql().Update(supplier);
        }
        public bool Delte(string supplierCode)
        {
            return new TB_SUPPLIERSSql().Delete(supplierCode);
        }
        public List<TB_SUPPLIERS> GetAll()
        {
            return new TB_SUPPLIERSSql().SelectAll();
        }

        public TB_SUPPLIERS GetById(string supplierCode)
        {
            return new TB_SUPPLIERSSql().SelectByPrimaryKey(supplierCode);
        }
    }
}
