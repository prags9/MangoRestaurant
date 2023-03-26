namespace Mango.Services.OrderAPI.Models
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public double OrderTotal { get; set; }
        public double DiscountTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime PickUpDataTime { get; set; }
        public string CardNUmber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonthYear { get; set; }
        public int CartTotalItems { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public bool paymentStatus { get; set; } = false;
        public DateTime OrderTime { get; set; }
    }
}
