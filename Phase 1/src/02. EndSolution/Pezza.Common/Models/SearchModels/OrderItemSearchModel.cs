namespace Pezza.Common.Models.SearchModels
{
    public class OrderItemSearchModel
    {
        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public OrderSearchModel OrderSearchModel { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}