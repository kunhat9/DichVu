using CORE.Base;

namespace CORE.Views
{
    public class ReportRevenue : BusinessObject
    {
        public string RevenueType { get; set; } // SERVICE / PRODUCT
        public int RevenueTotal { get; set; } // Số đơn hàng
        public int RevenueMoney { get; set; } // Số tiền
    }
}
