using CORE.Internal;
using CORE.Tables;
using System.Collections.Generic;
using System;

namespace CORE.Services
{
    public class TB_SERVICE_DETAILSFactory
    {
        public List<TB_SERVICE_DETAILS> GetAllByServiceId(int serviceId)
        {
            return new TB_SERVICE_DETAILSSql().FilterByField("SrvDetailServiceId", serviceId);
        }

        public List<TB_SERVICE_DETAILS> GetAll()
        {
            return new TB_SERVICE_DETAILSSql().SelectAll();
        }
    }
}
