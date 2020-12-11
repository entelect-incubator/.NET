namespace Pezza.Core.Order.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetOrdersQuery : IRequest<ListResult<Common.Entities.Order>>
    {
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ListResult<Common.Entities.Order>>
    {
        private readonly IDataAccess<Common.Entities.Order> dataAcess;

        public GetOrdersQueryHandler(IDataAccess<Common.Entities.Order> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync();

            return ListResult<Common.Entities.Order>.Success(search);
        }
    }
}
