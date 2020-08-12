using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Models
{
    public class SYS_User
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Sex { get; set; }
        public string Avatar { get; set; }
        public string Barcode { get; set; }
        public string BarcodeType { get; set; }

        public string Address { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }

        public string UserType { get; set; }
        public string UserData { get; set; }
        public int RewardPoint { get; set; }
        public int IsVerified { get; set; }
        public string Status { get; set; }
        public DateTime ValueDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public List<PackageUser> listPackage { get; set; }
    }
    public class PackageUser
    {
        public string Code { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public string StatusPackage { get; set; }
    }
}
