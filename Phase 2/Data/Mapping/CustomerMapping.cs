namespace Pezza.Common.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class CustomerMapping
    {
        public static CustomerDTO Map(this Customer entity) =>
           (entity != null ) ? new CustomerDTO
           {
               Id = entity.Id,
               Address = entity.Address,
               City = entity.Address,
               ContactPerson = entity.ContactPerson,
               DateCreated = entity.DateCreated,
               Email = entity.Email,
               Name = entity.Name,
               Orders = entity.Orders.Map(),
               Phone = entity.Phone,
               Province = entity.Province,
               ZipCode = entity.ZipCode
           } : null;

        public static IEnumerable<CustomerDTO> Map(this IEnumerable<Customer> entity) =>
           entity.Select(x => x.Map());

        public static Customer Map(this CustomerDTO dto) =>
           (dto != null) ? new Customer
           {
               Id = dto.Id,
               Address = dto.Address,
               City = dto.Address,
               ContactPerson = dto.ContactPerson,
               DateCreated = dto.DateCreated ?? DateTime.Now,
               Email = dto.Email,
               Name = dto.Name,
               Orders = dto.Orders.Map(),
               Phone = dto.Phone,
               Province = dto.Province,
               ZipCode = dto.ZipCode
           } : null;

        public static IEnumerable<Customer> Map(this IEnumerable<CustomerDTO> dto) =>
           dto.Select(x => x.Map());

        public static Customer Map(this CustomerDataDTO dto) =>
           (dto != null) ? new Customer
           {
               Address = dto.AddressBase.Address,
               City = dto.AddressBase.Address,
               ContactPerson = dto.ContactPerson,
               DateCreated = DateTime.Now,
               Email = dto.Email,
               Name = dto.Name,
               Phone = dto.Phone,
               Province = dto.AddressBase.Province,
               ZipCode = dto.AddressBase.ZipCode
           } : null;

        public static IEnumerable<Customer> Map(this IEnumerable<CustomerDataDTO> dto) =>
           dto.Select(x => x.Map());
    }
}
