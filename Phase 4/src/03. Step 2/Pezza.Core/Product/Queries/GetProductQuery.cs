namespace Pezza.Core.Product.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetProductQuery : IRequest<Result<ProductDTO>>
    {
        public int Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<ProductDTO>>
    {
        private readonly IDataAccess<ProductDTO> DataAccess;

        public GetProductQueryHandler(IDataAccess<ProductDTO> DataAccess) => this.DataAccess = DataAccess;

        public async Task<Result<ProductDTO>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var search = await this.DataAccess.GetAsync(request.Id);
            return Result<ProductDTO>.Success(search);
        }
    }
}
