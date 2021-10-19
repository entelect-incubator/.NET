using Pezza.Common.Models.Base;

namespace Pezza.Common.DTO
{
    public class OrderItemDTO : EntityBase
    {
        public int? OrderId { get; set; }

        public int? Quantity { get; set; }

        public int? ProductId { get; set; }

        public ProductDTO Product { get; set; }
    }
}
