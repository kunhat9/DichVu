using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Models
{
    public class USER_LOGIN_INFOR
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public List<USER_LOGIN_ORG> OrgList { get; set; }
    }

    public class USER_LOGIN_ORG : BusinessObject
    {
        public int OrgId { get; set; }

        public string OrgName { get; set; }

        public string OrgAddress { get; set; }

        public string OrgStatus { get; set; }
    }
}
