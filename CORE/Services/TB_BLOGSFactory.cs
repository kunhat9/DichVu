using CORE.Internal;
using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class TB_BLOGSFactory
    {
        public int Insert(TB_BLOGS blog)
        {
            return int.Parse(new TB_BLOGSSql().InsertReturnId(blog));
        }
        public bool Update(TB_BLOGS blog)
        {
            return new TB_BLOGSSql().Update(blog);
        }
        public bool Delte(int blogId)
        {
            return new TB_BLOGSSql().Delete(blogId);
        }
        public List<TB_BLOGS> GetAll()
        {
            return new TB_BLOGSSql().SelectAll();
        }

        public TB_BLOGS GetById(int blog_id)
        {
            return new TB_BLOGSSql().SelectByPrimaryKey(blog_id);
        }
    }
}
