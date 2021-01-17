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
        private readonly IDatabaseContext databaseContext;

        private readonly IMapper mapper;

        public CustomerDataAccess(IDatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<CustomerDTO> GetAsync(int id)
            => this.mapper.Map<CustomerDTO>(await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ListResult<CustomerDTO>> GetAllAsync(Entity searchBase)
        {
            var searchModel = (CustomerDTO)searchBase;
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Customers.Select(x => x)
                .AsNoTracking()
                .FilterByName(searchModel.Name)
                .FilterByAddress(searchModel.Address?.Address)
                .FilterByCity(searchModel.Address?.City)
                .FilterByProvince(searchModel.Address?.Province)
                .FilterByZipCode(searchModel.Address?.ZipCode)
                .FilterByPhone(searchModel.Phone)
                .FilterByEmail(searchModel.Email)
                .FilterByContactPerson(searchModel.ContactPerson)
                .FilterByDateCreated(searchModel.DateCreated)

                .OrderBy(searchModel.OrderBy);

            var count = entities.Count();
            var paged = this.mapper.Map<List<CustomerDTO>>(await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync());

            return ListResult<CustomerDTO>.Success(paged, count);
        }

        public async Task<CustomerDTO> SaveAsync(CustomerDTO entity)
        {
            this.databaseContext.Customers.Add(this.mapper.Map<Customer>(entity));
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<CustomerDTO> UpdateAsync(CustomerDTO entity)
        {
            var findEntity = await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == entity.Id);
            findEntity.Name = !string.IsNullOrEmpty(entity?.Name) ? entity?.Name : findEntity.Name;
            findEntity.Address = !string.IsNullOrEmpty(entity?.Address?.Address) ? entity?.Address?.Address : findEntity.Address;
            findEntity.City = !string.IsNullOrEmpty(entity?.Address?.City) ? entity?.Address?.City : findEntity.City;
            findEntity.Province = !string.IsNullOrEmpty(entity?.Address?.Province) ? entity?.Address?.Province : findEntity.Province;
            findEntity.ZipCode = !string.IsNullOrEmpty(entity?.Address?.ZipCode) ? entity?.Address?.ZipCode : findEntity.ZipCode;
            findEntity.Phone = !string.IsNullOrEmpty(entity?.Phone) ? entity?.Phone : findEntity.Phone;
            findEntity.ContactPerson = !string.IsNullOrEmpty(entity?.ContactPerson) ? entity?.ContactPerson : findEntity.ContactPerson;
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