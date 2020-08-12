using CORE.Internal;
using CORE.Internal.View;
using CORE.Tables;
using CORE.Views;
using System.Collections.Generic;
using System;

namespace CORE.Services
{
    public class TB_SERVICESFactory
    {
        public string Insert(TB_SERVICES blog)
        {
            return new TB_SERVICESSql().InsertReturnId(blog);
        }
        public bool Update(TB_SERVICES blog)
        {
            return new TB_SERVICESSql().Update(blog);
        }
        public TB_SERVICES GetById(int serviceId)
        {
            return new TB_SERVICESSql().SelectByPrimaryKey(serviceId);
        }
        public List<TB_SERVICES> GetAll()
        {
            return new TB_SERVICESSql().SelectAll();
        }

        public bool Insert(string xml, out string edesc)
        {
            string ecode;
            new TB_SERVICESSql().SelectFromStore(out ecode, out edesc, AppSettingKeys.SP_INSERT_SERVICE, xml);
            return (ecode == "00");
        }

        public List<TB_SERVICE_DETAILS> GetAllDetails(int id)
        {
            return new TB_SERVICE_DETAILSSql().FilterByField("SrvDetailServiceId", id);
        }
    }
}
