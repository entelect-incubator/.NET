namespace Pezza.Common.Mapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class RestaurantMapping
    {
        public static RestaurantDTO Map(this Restaurant entity) =>
           (entity != null) ? new RestaurantDTO
           {
               Id = entity.Id,
               Address = entity.Address,
               City = entity.Address,
               Description = entity.Description,
               DateCreated = entity.DateCreated,
               IsActive = entity.IsActive,
               Name = entity.Name,
               Orders = entity.Orders.Map(),
               PictureUrl = entity.PictureUrl,
               PostalCode = entity.PostalCode,
               Province = entity.Province

           } : null;

        public static IEnumerable<RestaurantDTO> Map(this IEnumerable<Restaurant> entity) =>
           entity.Select(x => x.Map());

        public static Restaurant Map(this RestaurantDTO dto) =>
           (dto != null) ? new Restaurant
           {
               Id = dto.Id,
               Address = dto.Address,
               City = dto.Address,
               Description = dto.Description,
               DateCreated = dto.DateCreated,
               IsActive = dto.IsActive ?? dto.IsActive.Value,
               Name = dto.Name,
               Orders = dto.Orders.Map(),
               PictureUrl = dto.PictureUrl,
               PostalCode = dto.PostalCode,
               Province = dto.Province
           } : null;

        public static IEnumerable<Restaurant> Map(this IEnumerable<RestaurantDTO> dto) =>
           dto.Select(x => x.Map());
    }
}
