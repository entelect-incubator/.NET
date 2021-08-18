namespace Pezza.DataAccess.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Extensions;
    using Pezza.Common.Filter;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class CustomerDataAccess : IDataAccess<CustomerDTO>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public CustomerDataAccess(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<CustomerDTO> GetAsync(int id)
            => this.mapper.Map<CustomerDTO>(await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<CustomerDTO>> GetAllAsync(CustomerDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OrderBy))
            {
                dto.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Customers.Select(x => x)
                .AsNoTracking()
                .FilterByName(dto.Name)
                .FilterByAddress(dto.Address?.Address)
                .FilterByCity(dto.Address?.City)
                .FilterByProvince(dto.Address?.Province)
                .FilterByPostalCode(dto.Address?.PostalCode)
                .FilterByPhone(dto.Phone)
                .FilterByEmail(dto.Email)
                .FilterByContactPerson(dto.ContactPerson)

                .OrderBy(dto.OrderBy);

            var count = entities.Count();
            var paged = this.mapper.Map<List<CustomerDTO>>(await entities.ApplyPaging(dto.PagingArgs).ToListAsync());
            return ListResult<CustomerDTO>.Success(paged, count);
        }

        public async Task<CustomerDTO> SaveAsync(CustomerDTO dto)
        {
            var entity = this.mapper.Map<Customer>(dto);
            this.databaseContext.Customers.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            dto.Id = entity.Id;

            return dto;
        }

        public async Task<CustomerDTO> UpdateAsync(CustomerDTO dto)
        {
            var findEntity = await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (findEntity == null)
            {
                return null;
            }

            findEntity.Name = !string.IsNullOrEmpty(dto?.Name) ? dto?.Name : findEntity.Name;
            findEntity.Address = !string.IsNullOrEmpty(dto?.Address?.Address) ? dto?.Address?.Address : findEntity.Address;
            findEntity.City = !string.IsNullOrEmpty(dto?.Address?.City) ? dto?.Address?.City : findEntity.City;
            findEntity.Province = !string.IsNullOrEmpty(dto?.Address?.Province) ? dto?.Address?.Province : findEntity.Province;
            findEntity.PostalCode = !string.IsNullOrEmpty(dto?.Address?.PostalCode) ? dto?.Address?.PostalCode : findEntity.PostalCode;
            findEntity.Phone = !string.IsNullOrEmpty(dto?.Phone) ? dto?.Phone : findEntity.Phone;
            findEntity.ContactPerson = !string.IsNullOrEmpty(dto?.ContactPerson) ? dto?.ContactPerson : findEntity.ContactPerson;
            this.databaseContext.Customers.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<CustomerDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            this.databaseContext.Customers.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return result == 1;
        }
    }
}