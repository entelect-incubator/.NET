namespace Pezza.Core.Product.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetProductsQuery : IRequest<ListResult<Common.Entities.Product>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ListResult<Common.Entities.Product>>
    {
        private readonly IDataAccess<Common.Entities.Product> dataAcess;

        public GetProductsQueryHandler(IDataAccess<Common.Entities.Product> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync();

            return ListResult<Common.Entities.Product>.Success(search);
        }
    }
}
