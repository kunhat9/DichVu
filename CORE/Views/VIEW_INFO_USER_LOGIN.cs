using CORE.Base;

namespace CORE.Views
{
    public class VIEW_INFO_USER_LOGIN : BusinessObject
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserFullName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserAddress { get; set; }
        public string UserType { get; set; }
        public string UserStatus { get; set; }
        public string UserNote { get; set; }
    }
}
