namespace Pezza.Core.Customer.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.Common.Models.Base;
    using Pezza.DataAccess.Contracts;

    public interface ICreateCommand<TDTO> : IRequest<bool> where TDTO : EntityBase, new()
    {
        TDTO Data { get; set; }
    }

    public class CreateCommandHandler<TEntity, TDTO, TCommand> : IRequestHandler<TCommand, bool>
        where TEntity : EntityBase, new()
        where TDTO : EntityBase, new()
        where TCommand : class, ICreateCommand<TEntity>, new()
    {
        private readonly IDataAccess<TEntity> dataAccess;

        private readonly IMapper mapper;

        public CreateCommandHandler(IDataAccess<TEntity> dataAccess, IMapper mapper)
            => (this.dataAccess, this.mapper) = (dataAccess, mapper);

        public async Task<Result<TDTO>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = this.mapper.Map<TEntity>(request.Data);
            await this.dataAccess.SaveAsync(entity);
            var outcome = await this.dataAccess.Complete();
            return (outcome > 0) ? Result<TDTO>.Success(this.mapper.Map<TDTO>(entity) : Result<TDTO>.Failure($"Error creating a {nameof(TDTO)}");
        }
    }
}
