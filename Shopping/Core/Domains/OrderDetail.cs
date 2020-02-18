using Common.Core.Domain;

namespace Shopping.Core.Domains
{
    public class OrderDetail : AuditEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
