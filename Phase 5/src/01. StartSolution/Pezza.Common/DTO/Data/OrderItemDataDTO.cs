namespace Pezza.Common.DTO
{
    using Pezza.Common.DTO.Data;

    public class OrderItemDataDTO : SearchBase
    {
        public int Quantity { get; set; }

        public int? ProductId { get; set; }

        public ProductDataDTO Product { get; set; }
    }
}
