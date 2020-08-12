using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAdmin.Models
{
    public class InfoPackageUser
    {
        public string PACKAGE_ID { get; set; }
        public string PACKAGE_NAME { get; set; }
        public string PACKAGE_TYPE { get; set; }
        public string TOTAL_TIME { get; set; }
        public string USED_TIME { get; set; }
        public string REST_TIME { get; set; }
    }
}