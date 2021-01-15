namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.DTO.Data;

    public class StockDataDTO : SearchBase
    {
        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public double? ValueOfMeasure { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Comment { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
