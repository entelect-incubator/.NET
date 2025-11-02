namespace Core.Product.Queries;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.DTO;
using Pezza.Pezza.Common.Models;
using DataAccess;

public sealed class GetProductQuery : ICommand<Result<ProductDTO>>
{
    public int Id { get; set; }
}

public sealed class GetProductQueryHandler : ICommandHandler<GetProductQuery, Result<ProductDTO>>
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

