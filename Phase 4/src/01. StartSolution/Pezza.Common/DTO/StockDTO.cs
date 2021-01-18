namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class StockDTO : Entity, Data.ISearchBase
    {
        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public double? ValueOfMeasure { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Comment { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
