namespace Pezza.Common.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class OrderMapping
    {
        public static OrderDTO Map(this Order entity) =>
           (entity != null) ? new OrderDTO
           {
               Id = entity.Id,
               Amount = entity.Amount,
               CustomerId = entity.CustomerId,
               Customer = entity.Customer?.Map(),
               DateCreated = entity.DateCreated,
               RestaurantId = entity.RestaurantId,
               Restaurant = entity.Restaurant?.Map(),
               Completed = entity.Completed,
               OrderItems = entity.OrderItems?.Map()
           } : null;

        public static IEnumerable<OrderDTO> Map(this IEnumerable<Order> entity) =>
           entity.Select(x => x.Map());

        public static ICollection<OrderDTO> Map(this ICollection<Order> entity)
            => !entity.Any() ? new List<OrderDTO>() : entity.Select(x => x.Map()).ToList();

        public static Order Map(this OrderDTO dto) =>
           (dto != null) ? new Order
           {
               Id = dto.Id,
               Amount = dto.Amount ?? dto.Amount.Value,
               CustomerId = dto.CustomerId ?? dto.CustomerId.Value,
               DateCreated = dto.DateCreated,
               RestaurantId = dto.RestaurantId ?? dto.RestaurantId.Value,
               Completed = dto.Completed ?? dto.Completed.Value,
               OrderItems = dto.OrderItems.Map()
           } : null;

        public static IEnumerable<Order> Map(this IEnumerable<OrderDTO> dto) =>
           dto.Select(x => x.Map());

        public static ICollection<Order> Map(this ICollection<OrderDTO> dto) =>
           dto.Select(x => x.Map()).ToList();

        public static Order Map(this OrderDataDTO dto) =>
          (dto != null) ? new Order
          {
              Amount = dto.Amount ?? dto.Amount.Value,
              CustomerId = dto.CustomerId ?? 0,
              Customer = dto.Customer.Map(),
              DateCreated = DateTime.Now,
              RestaurantId = dto.RestaurantId ?? 0,
              Completed = dto.Completed ?? false,
              OrderItems = dto.OrderItems.Map()
          } : null;

        public static IEnumerable<Order> Map(this IEnumerable<OrderDataDTO> dto) =>
           dto.Select(x => x.Map());

        public static ICollection<Order> Map(this ICollection<OrderDataDTO> dto) =>
           dto.Select(x => x.Map()).ToList();
    }
}
