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

    public class NotifyDataAccess : IDataAccess<NotifyDTO>
    {
        private readonly IDatabaseContext databaseContext;

        private readonly IMapper mapper;

        public NotifyDataAccess(IDatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<NotifyDTO> GetAsync(int id)
            => this.mapper.Map<NotifyDTO>(await this.databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<List<NotifyDTO>> GetAllAsync()
        {
            var entities = await this.databaseContext.Notify.Select(x => x).AsNoTracking().ToListAsync();
            return this.mapper.Map<List<NotifyDTO>>(entities);
        }

        public async Task<NotifyDTO> SaveAsync(NotifyDTO entity)
        {
            this.databaseContext.Notify.Add(this.mapper.Map<Notify>(entity));
            await this.databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<NotifyDTO> UpdateAsync(NotifyDTO entity)
        {
            var findEntity = await this.databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == entity.Id);

            findEntity.CustomerId = entity.CustomerId ?? findEntity.CustomerId;
            findEntity.Email = !string.IsNullOrEmpty(entity.Email) ? entity.Email : findEntity.Email;
            findEntity.Sent = entity.Sent ?? findEntity.Sent;
            findEntity.Retry = entity.Retry ?? findEntity.Retry;

            this.databaseContext.Notify.Update(findEntity);
            await this.databaseContext.SaveChangesAsync();

            return this.mapper.Map<NotifyDTO>(findEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await this.databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == id);
            this.databaseContext.Notify.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();

            return (result == 1);
        }
    }
}