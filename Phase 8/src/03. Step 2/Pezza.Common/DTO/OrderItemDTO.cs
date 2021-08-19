namespace Pezza.Common.DTO
{
    using Pezza.Common.DTO.Data;

    public class OrderItemDTO : SearchBase
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public int? Quantity { get; set; }

        public int? ProductId { get; set; }

        public ProductDTO Product { get; set; }
    }
}
