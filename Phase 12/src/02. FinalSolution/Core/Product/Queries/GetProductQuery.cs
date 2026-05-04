namespace Core.Product.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetProductQuery : IQuery<Result<ProductDTO>>
{
    public int Id { get; set; }
}

public sealed class GetProductQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetProductQuery, Result<ProductDTO>>
{
    public async Task<Result<ProductDTO>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var result = (await databaseContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken))?.ToDto();
        return Result<ProductDTO>.Success(result);
    }
}
