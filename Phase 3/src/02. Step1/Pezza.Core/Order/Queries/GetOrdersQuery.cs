namespace Pezza.Core.Order.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetOrdersQuery : IRequest<ListResult<OrderDTO>>
    {
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ListResult<OrderDTO>>
    {
        private readonly IDataAccess<OrderDTO> dataAcess;

        public GetOrdersQueryHandler(IDataAccess<OrderDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync();
            return ListResult<OrderDTO>.Success(search);
        }
    }
}
