namespace Pezza.Common.Entities
{
    public class OrderItem : Entity
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
