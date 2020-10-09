namespace Pezza.Core.Order.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class GetOrdersQuery : IRequest<ListResult<Common.Entities.Order>>
    {
        public OrderSearchModel OrderSearchModel { get; set; }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ListResult<Common.Entities.Order>>
    {
        private readonly IOrderDataAccess dataAcess;

        public GetOrdersQueryHandler(IOrderDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.OrderSearchModel);

            return ListResult<Common.Entities.Order>.Success(search);
        }
    }
}
