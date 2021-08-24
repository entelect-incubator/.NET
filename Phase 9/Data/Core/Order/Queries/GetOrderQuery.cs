namespace Pezza.Core.Order.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetOrderQuery : IRequest<Result<Common.Entities.Order>>
    {
        public int Id { get; set; }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Result<Common.Entities.Order>>
    {
        private readonly IDataAccess<Common.Entities.Order> DataAccess;

        public GetOrderQueryHandler(IDataAccess<Common.Entities.Order> DataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<Common.Entities.Order>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAccess.GetAsync(request.Id);

            return Result<Common.Entities.Order>.Success(search);
        }
    }
}
