namespace Pezza.Common.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class StockMapping
    {
        public static StockDTO Map(this Stock stock) =>
           (stock != null ) ? new StockDTO
           {
               Id = stock.Id,
               ExpiryDate = stock.ExpiryDate,
               Name = stock.Name,
               Quantity = stock.Quantity,
               UnitOfMeasure = stock.UnitOfMeasure,
               ValueOfMeasure = stock.ValueOfMeasure,
               Comment = stock.Comment
           } : null;

        public static IEnumerable<StockDTO> Map(this IEnumerable<Stock> stock) =>
           stock.Select(x => x.Map());

        public static Stock Map(this StockDTO stock) =>
           (stock != null) ? new Stock
           {
               Id = stock.Id,
               ExpiryDate = stock.ExpiryDate,
               Name = stock.Name,
               Quantity = stock.Quantity ?? 0,
               UnitOfMeasure = stock.UnitOfMeasure,
               ValueOfMeasure = stock.ValueOfMeasure,
               Comment = stock.Comment,
               DateCreated = DateTime.Now
           } : null;

        public static IEnumerable<Stock> Map(this IEnumerable<StockDTO> stock) =>
           stock.Select(x => x.Map());

        public static Stock Map(this StockDataDTO stock) =>
          (stock != null) ? new Stock
          {
              ExpiryDate = stock.ExpiryDate,
              Name = stock.Name,
              Quantity = stock.Quantity ?? 0,
              UnitOfMeasure = stock.UnitOfMeasure,
              ValueOfMeasure = stock.ValueOfMeasure,
              Comment = stock.Comment,
              DateCreated = DateTime.Now
          } : null;

        public static IEnumerable<Stock> Map(this IEnumerable<StockDataDTO> stock) =>
           stock.Select(x => x.Map());
    }
}
