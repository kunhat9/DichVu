using CORE.Internal;
using CORE.Tables;
using System;
using System.Collections.Generic;

namespace CORE.Services
{
    public class TB_PRODUCTSFactory
    {
        public bool Insert(TB_PRODUCTS product)
        {
            return new TB_PRODUCTSSql().Insert(product, true);
        }
        public bool Update(TB_PRODUCTS product)
        {
            return new TB_PRODUCTSSql().Update(product);
        }
        public bool Delte(string productCode)
        {
            return new TB_PRODUCTSSql().Delete(productCode);
        }
        public List<TB_PRODUCTS> GetAll()
        {
            return new TB_PRODUCTSSql().SelectAll();
        }

        public TB_PRODUCTS GetById(string productCode)
        {
            return new TB_PRODUCTSSql().SelectByPrimaryKey(productCode);
        }
    }
}
