using CORE.Internal;
using CORE.Internal.View;
using CORE.Tables;
using CORE.Views;
using System.Collections.Generic;
using System;

namespace CORE.Services
{
    public class TB_TYPESFactory
    {
        public bool Insert(TB_TYPES blog)
        {
            return new TB_TYPESSql().Insert(blog, true);
        }
        public bool Update(TB_TYPES blog)
        {
            return new TB_TYPESSql().Update(blog);
        }
        public TB_TYPES GetById(string typeId)
        {
            return new TB_TYPESSql().SelectByPrimaryKey(typeId);
        }
        public List<TB_TYPES> GetAll()
        {
            return new TB_TYPESSql().SelectAll();
        }

        public List<TB_TYPE_DETAILS> GetAllDetails(string typeCode)
        {
            return new TB_TYPE_DETAILSSql().FilterByField("TypeDetailTypeCode", typeCode);
        }

        public bool Insert(string xml)
        {
            string ecode, edesc;
            new TB_TYPE_DETAILSSql().SelectFromStore(out ecode, out edesc, AppSettingKeys.SP_INSERT_TYPE_DETAIL, xml);
            return (ecode == "00");
        }
    }
}
