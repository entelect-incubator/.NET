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
        private readonly IDataAccess<OrderDTO> dataAcess;

        public GetOrderQueryHandler(IDataAccess<OrderDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<OrderDTO>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAsync(request.Id);
            return Result<OrderDTO>.Success(search);
        }
    }
}
