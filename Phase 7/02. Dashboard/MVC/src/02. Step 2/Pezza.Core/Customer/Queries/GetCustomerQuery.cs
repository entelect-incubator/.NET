namespace Pezza.Core.Customer.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetCustomerQuery : IRequest<Result<CustomerDTO>>
    {
        public int Id { get; set; }
    }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Result<CustomerDTO>>
    {
        private readonly IDataAccess<CustomerDTO> dto;

        public GetCustomerQueryHandler(IDataAccess<CustomerDTO> dto) => this.dto = dto;

        public async Task<Result<CustomerDTO>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dto.GetAsync(request.Id);
            return Result<CustomerDTO>.Success(search);
        }
    }
}
