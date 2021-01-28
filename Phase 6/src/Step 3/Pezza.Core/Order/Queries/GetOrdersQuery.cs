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
        public OrderDTO SearchModel { get; set; }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ListResult<OrderDTO>>
    {
        private readonly IDataAccess<OrderDTO> dataAcess;

        public GetOrdersQueryHandler(IDataAccess<OrderDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            => await this.dataAcess.GetAllAsync(request.SearchModel);
    }
}
