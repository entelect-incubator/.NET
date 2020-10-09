namespace Pezza.Common.Mapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class OrderItemMapping
    {
        public static OrderItemDTO Map(this OrderItem entity) =>
           (entity != null) ? new OrderItemDTO
           {
               Id = entity.Id,
               OrderId = entity.OrderId,
               ProductId = entity.ProductId,
               Quantity = entity.Quantity
           } : null;

        public static IEnumerable<OrderItemDTO> Map(this IEnumerable<OrderItem> entity) =>
           entity.Select(x => x.Map());

        public static ICollection<OrderItemDTO> Map(this ICollection<OrderItem> entity) =>
           entity.Select(x => x.Map()).ToList();

        public static OrderItem Map(this OrderItemDTO dto) =>
           (dto != null) ? new OrderItem
           {
               Id = dto.Id,
               OrderId = dto.OrderId ?? dto.OrderId.Value,
               ProductId = dto.ProductId ?? dto.ProductId.Value,
               Quantity = dto.Quantity ?? dto.Quantity.Value
           } : null;

        public static IEnumerable<OrderItem> Map(this IEnumerable<OrderItemDTO> dto) =>
           dto.Select(x => x.Map());

        public static ICollection<OrderItem> Map(this ICollection<OrderItemDTO> dto) =>
           dto.Select(x => x.Map()).ToList();
    }
}
