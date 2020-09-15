namespace Pezza.Core.Customer.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetCustomerQuery : IRequest<Result<Common.Entities.Customer>>
    {
        public int Id { get; set; }
    }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Result<Common.Entities.Customer>>
    {
        private readonly ICustomerDataAccess dataAcess;

        public GetCustomerQueryHandler(ICustomerDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Customer>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAsync(request.Id);

            return Result<Common.Entities.Customer>.Success(search);
        }
    }
}
