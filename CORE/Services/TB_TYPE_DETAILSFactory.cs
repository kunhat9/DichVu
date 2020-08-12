using CORE.Internal;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class TB_TYPE_DETAILSFactory
    {
        public List<TB_TYPE_DETAILS> GetByTypeId(int typeId)
        {
            return new TB_TYPE_DETAILSSql().FilterByField(TB_TYPE_DETAILS.TB_TYPE_DETAILS_Field.TypeDetailTypeCode.ToString(),typeId);
        }
        public TB_TYPE_DETAILS GetById(int detailsId)
        {
            return new TB_TYPE_DETAILSSql().SelectByPrimaryKey(detailsId);
        }
    }
}
