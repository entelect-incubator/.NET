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
        private readonly IDataAccess<OrderDTO> dataAccess;

        public GetOrdersQueryHandler(IDataAccess<OrderDTO> dataAccess) => this.dataAccess = dataAccess;

        public async Task<ListResult<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            => await this.dataAccess.GetAllAsync(request.SearchModel);
    }
}
