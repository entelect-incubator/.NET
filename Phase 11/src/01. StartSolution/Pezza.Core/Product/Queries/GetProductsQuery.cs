namespace Core.Product.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.DTO;
using Pezza.Common.Extensions;
using Pezza.Common.Filters;
using Pezza.Pezza.Common.Models;
using DataAccess;

public sealed class GetProductsQuery : IQuery<Result<IEnumerable<ProductDTO>>>
{
    public ProductDTO? Data { get; set; }
}

public sealed class GetProductsQueryHandler(DatabaseContext databaseContext, IMapper mapper)
    : IQueryHandler<GetProductsQuery, Result<IEnumerable<ProductDTO>>>\n{\n    public async Task<Result<IEnumerable<ProductDTO>>> HandleAsync(GetProductsQuery request, CancellationToken cancellationToken)
    {
        if (request.Data == null)
        {
            return Result<IEnumerable<ProductDTO>>.Failure("Product search data is required");
        }

        var dto = request.Data;
        dto.OrderBy ??= "DateCreated desc";

        var entities = databaseContext.Products.Select(x => x)
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
        var paged = mapper.Map<List<ProductDTO>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

        return Result<IEnumerable<ProductDTO>>.Success(paged, count);
    }
}

