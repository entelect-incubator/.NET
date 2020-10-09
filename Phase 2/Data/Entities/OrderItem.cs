namespace Pezza.Common.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public Product Product { get; set; }
    }
}
