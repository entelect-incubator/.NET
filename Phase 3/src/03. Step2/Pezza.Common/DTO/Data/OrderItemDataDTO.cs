namespace Pezza.Common.DTO
{
    public class OrderItemDataDTO
    {
        public int Quantity { get; set; }

        public int? ProductId { get; set; }

        public ProductDataDTO Product { get; set; }
    }
}
