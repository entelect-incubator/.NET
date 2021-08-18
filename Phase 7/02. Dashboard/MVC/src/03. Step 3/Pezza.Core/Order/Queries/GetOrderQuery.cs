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
        private readonly IDataAccess<OrderDTO> dto;

        public GetOrderQueryHandler(IDataAccess<OrderDTO> dto) => this.dto = dto;

        public async Task<Result<OrderDTO>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dto.GetAsync(request.Id);
            return Result<OrderDTO>.Success(search);
        }
    }
}
