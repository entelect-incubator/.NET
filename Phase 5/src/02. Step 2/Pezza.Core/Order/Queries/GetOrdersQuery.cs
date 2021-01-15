namespace Pezza.Core.Order.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetOrdersQuery : IRequest<ListResult<OrderDTO>>
    {
        public OrderDataDTO SearchModel { get; set; }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ListResult<OrderDTO>>
    {
        private readonly IDataAccess<Common.Entities.Order> dataAcess;

        public GetOrdersQueryHandler(IDataAccess<Common.Entities.Order> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.SearchModel);

            return ListResult<OrderDTO>.Success(search.Data.Map(), search.Count);
        }
    }
}
