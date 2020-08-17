namespace Pezza.Core.Customer.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class GetCustomerQuery : IRequest<ListResult<Common.Entities.Customer>>
    {
        public CustomerSearchModel CustomerSearchModel { get; set; }
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomerQuery, ListResult<Common.Entities.Customer>>
    {
        private readonly ICustomerDataAccess dataAcess;

        public GetCustomersQueryHandler(ICustomerDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Customer>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.CustomerSearchModel);

            return ListResult<Common.Entities.Customer>.Success(search);
        }
    }
}
