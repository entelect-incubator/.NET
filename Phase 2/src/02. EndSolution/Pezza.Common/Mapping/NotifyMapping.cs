namespace Pezza.Common.Mapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class NotifyMapping
    {
        public static NotifyDTO Map(this Notify entity) =>
           (entity != null) ? new NotifyDTO
           {
               Id = entity.Id,
               CustomerId = entity.CustomerId,
               DateSent = entity.DateSent,
               Email = entity.Email,
               Retry = entity.Retry,
               Sent = entity.Sent
           } : null;

        public static IEnumerable<NotifyDTO> Map(this IEnumerable<Notify> entity) =>
           entity.Select(x => x.Map());

        public static Notify Map(this NotifyDTO dto) =>
           (dto != null) ? new Notify
           {
               Id = dto.Id,
               CustomerId = dto.CustomerId ?? dto.CustomerId.Value,
               DateSent = dto.DateSent ?? dto.DateSent.Value,
               Email = dto.Email,
               Retry = dto.Retry ?? dto.Retry.Value,
               Sent = dto.Sent ?? dto.Sent.Value
           } : null;

        public static IEnumerable<Notify> Map(this IEnumerable<NotifyDTO> dto) =>
           dto.Select(x => x.Map());
    }
}
