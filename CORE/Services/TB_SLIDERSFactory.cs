using CORE.Internal;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class TB_SLIDERSFactory
    {
        public int Insert(TB_SLIDERS slider)
        {
            return int.Parse(new TB_SLIDERSSql().InsertReturnId(slider));
        }
        public bool Update(TB_SLIDERS slider)
        {
            return new TB_SLIDERSSql().Update(slider);
        }
        public bool Delte(int sliderId)
        {
            return new TB_SLIDERSSql().Delete(sliderId);
        }
        public List<TB_SLIDERS> GetAll()
        {
            return new TB_SLIDERSSql().SelectAll();
        }

        public TB_SLIDERS GetById(int sliderId)
        {
            return new TB_SLIDERSSql().SelectByPrimaryKey(sliderId);
        }
    }
}
