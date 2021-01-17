namespace Pezza.Common.DTO
{
    using Pezza.Common.Entities;

    public class OrderItemDTO : Entity
    {
        public int? OrderId { get; set; }

        public int? Quantity { get; set; }

        public int? ProductId { get; set; }

        public ProductDTO Product { get; set; }
    }
}
