using CORE.Internal;
using CORE.Tables;
using System;
using System.Collections.Generic;

namespace CORE.Services
{
    public class TB_CATEGORIESFactory
    {
        public bool Insert(TB_CATEGORIES category)
        {
            return new TB_CATEGORIESSql().Insert(category, true);
        }
        public bool Update(TB_CATEGORIES category)
        {
            return new TB_CATEGORIESSql().Update(category);
        }
        public bool Delte(string categoryCode)
        {
            return new TB_SUPPLIERSSql().Delete(categoryCode);
        }
        public List<TB_CATEGORIES> GetAll()
        {
            return new TB_CATEGORIESSql().SelectAll();
        }

        public TB_CATEGORIES GetById(string categoryCode)
        {
            return new TB_CATEGORIESSql().SelectByPrimaryKey(categoryCode);
        }
    }
}
