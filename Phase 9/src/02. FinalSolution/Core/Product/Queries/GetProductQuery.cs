namespace Core.Product.Queries;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Models;
using DataAccess;

public sealed class GetProductQuery : IQuery<Result<ProductDTO>>
{
    public int Id { get; set; }
}

public sealed class GetProductQueryHandler : IQueryHandler<GetProductQuery, Result<ProductDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetProductQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<ProductDTO>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var result = this.mapper.Map<ProductDTO>(await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        return Result<ProductDTO>.Success(result);
    }
}
