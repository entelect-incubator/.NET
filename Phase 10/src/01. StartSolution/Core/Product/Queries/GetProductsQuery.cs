namespace Core.Product.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Extensions;
using Common.Filters;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetProductsQuery : IQuery<Result<IEnumerable<ProductDTO>>>
{
    public ProductDTO Data { get; set; }
}

public sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, Result<IEnumerable<ProductDTO>>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetProductsQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<IEnumerable<ProductDTO>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        if (string.IsNullOrEmpty(dto.OrderBy))
        {
            dto.OrderBy = "DateCreated desc";
        }

        var entities = this.databaseContext.Products.Select(x => x)
            .AsNoTracking()
            .FilterByName(dto.Name)
            .FilterByDescription(dto.Description)
            .FilterByPictureUrl(dto.PictureUrl)
            .FilterByPrice(dto.Price)
            .FilterBySpecial(dto.Special)
            .FilterByOfferEndDate(dto.OfferEndDate)
            .FilterByOfferPrice(dto.OfferPrice)
            .FilterByIsActive(dto.IsActive)

            .OrderBy(dto.OrderBy);

        var count = await entities.CountAsync(cancellationToken);
        var paged = this.mapper.Map<List<ProductDTO>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

        return Result<IEnumerable<ProductDTO>>.Success(paged, count);
    }
}
