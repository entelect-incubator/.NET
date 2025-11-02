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

public sealed class GetProductsQuery : ICommand<ListResult<ProductDTO>>
{
    public ProductDTO Data { get; set; }
}

public sealed class GetProductsQueryHandler : ICommandHandler<GetProductsQuery, ListResult<ProductDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetProductsQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
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

        return ListResult<ProductDTO>.Success(paged, count);
    }
}

