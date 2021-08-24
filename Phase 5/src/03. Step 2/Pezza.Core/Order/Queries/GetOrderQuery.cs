namespace Pezza.Core.Order.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetOrderQuery : IRequest<Result<OrderDTO>>
    {
        public int Id { get; set; }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Result<OrderDTO>>
    {
        private readonly IDataAccess<OrderDTO> dataAccess;

        public GetOrderQueryHandler(IDataAccess<OrderDTO> dataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<OrderDTO>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAccess.GetAsync(request.Id);
            return Result<OrderDTO>.Success(search);
        }
    }
}
