namespace Pezza.Core.Product.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Extensions;
    using Pezza.Common.Filters;
    using Pezza.Common.Models;
    using Pezza.DataAccess;

    public class GetProductsQuery : IRequest<ListResult<ProductDTO>>
    {
        public ProductDTO Data { get; set; }
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ListResult<ProductDTO>>
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

            var count = entities.Count();
            var paged = this.mapper.Map<List<ProductDTO>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

            return ListResult<ProductDTO>.Success(paged, count);
        }
    }
}
