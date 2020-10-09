namespace Pezza.Common.Models.SearchModels
{
    using System;

    public class OrderSearchModel
    {
        public int? CustomerId { get; set; }

        public int? RestaurantId { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? DateCreated { get; set; }

        public OrderItemSearchModel OrderItemSearchModel { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}