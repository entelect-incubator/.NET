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
    using Pezza.DataAccess.Contracts;

    public class CustomerDataAccess : IDataAccess<CustomerDTO>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public CustomerDataAccess(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<CustomerDTO> GetAsync(int id)
            => this.mapper.Map<CustomerDTO>(await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<List<CustomerDTO>> GetAllAsync()
        {
            var entities = await this.databaseContext.Customers.Select(x => x).AsNoTracking().ToListAsync();
            return this.mapper.Map<List<CustomerDTO>>(entities);
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
            this.databaseContext.Customers.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}