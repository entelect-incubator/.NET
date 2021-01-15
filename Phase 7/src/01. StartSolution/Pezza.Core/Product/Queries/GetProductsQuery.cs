namespace Pezza.Core.Product.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetProductsQuery : IRequest<ListResult<ProductDTO>>
    {
        public ProductDataDTO SearchModel { get; set; }
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ListResult<ProductDTO>>
    {
        private readonly IDataAccess<Common.Entities.Product> dataAcess;

        public GetProductsQueryHandler(IDataAccess<Common.Entities.Product> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.SearchModel);

            return ListResult<ProductDTO>.Success(search.Data.Map(), search.Count);
        }
    }
}
