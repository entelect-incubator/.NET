namespace Pezza.Core.Product.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class GetProductQuery : IRequest<ListResult<Common.Entities.Product>>
    {
        public ProductSearchModel ProductSearchModel { get; set; }
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductQuery, ListResult<Common.Entities.Product>>
    {
        private readonly IProductDataAccess dataAcess;

        public GetProductsQueryHandler(IProductDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.ProductSearchModel);

            return ListResult<Common.Entities.Product>.Success(search);
        }
    }
}
