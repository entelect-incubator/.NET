namespace Pezza.Core.Product.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetProductsQuery : IRequest<ListResult<ProductDTO>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ListResult<ProductDTO>>
    {
        private readonly IDataAccess<ProductDTO> dataAccess;

        public GetProductsQueryHandler(IDataAccess<ProductDTO> dataAccess) => this.dataAccess = dataAccess;

        public async Task<ListResult<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAccess.GetAllAsync();
            return ListResult<ProductDTO>.Success(search, search.Count);
        }
    }
}
