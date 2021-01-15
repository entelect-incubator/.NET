namespace Pezza.Common.Mapping
{
    using System;
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
               PictureUrl = dto.PictureUrl,
               PostalCode = dto.PostalCode,
               Province = dto.Province
           } : null;

        public static IEnumerable<Restaurant> Map(this IEnumerable<RestaurantDTO> dto) =>
           dto.Select(x => x.Map());

        public static Restaurant Map(this RestaurantDataDTO dto) =>
           (dto != null) ? new Restaurant
           {
               Name = dto.Name,
               Description = dto.Description ?? string.Empty,
               DateCreated = DateTime.Now,
               PictureUrl = dto.PictureUrl,
               Address = dto.Address?.Address,
               City = dto.Address?.City,
               Province = dto.Address?.Province,
               PostalCode = dto.Address?.ZipCode,
               IsActive = dto.IsActive ?? true,
           } : null;

        public static IEnumerable<Restaurant> Map(this IEnumerable<RestaurantDataDTO> dto) =>
           dto.Select(x => x.Map());
    }
}
