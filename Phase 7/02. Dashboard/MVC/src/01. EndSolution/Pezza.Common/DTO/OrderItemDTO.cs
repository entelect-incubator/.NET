namespace Pezza.Common.DTO
{
    using Pezza.Common.DTO.Data;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class OrderItemDTO : Entity, ISearchBase
    {
        public int? OrderId { get; set; }

        public int? Quantity { get; set; }

        public int? ProductId { get; set; }

        public ProductDTO Product { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
