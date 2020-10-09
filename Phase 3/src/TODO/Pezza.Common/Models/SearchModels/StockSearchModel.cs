namespace Pezza.Common.Models.SearchModels
{
    using System;

    public class StockSearchModel
    {
        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal? ValueOfMeasure { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? DateCreated { get; set; }

        public string Comment { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}