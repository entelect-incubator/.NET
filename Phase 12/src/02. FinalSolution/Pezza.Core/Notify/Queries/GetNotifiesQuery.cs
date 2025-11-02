namespace Core.Notify.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Extensions;
using Common.Filters;
using Common.Models;
using DataAccess;

public sealed class GetNotifiesQuery : IQuery<ListResult<NotifyDTO>>
{
    public NotifyDTO Data { get; set; }
}

public sealed class GetNotifiesQueryHandler : IQueryHandler<GetNotifiesQuery, ListResult<NotifyDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetNotifiesQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<NotifyDTO>> HandleAsync(GetNotifiesQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        if (string.IsNullOrEmpty(dto.OrderBy))
        {
            dto.OrderBy = "DateSent desc";
        }

        var entities = this.databaseContext.Notify.Select(x => x)
            .AsNoTracking()
            .FilterByCustomerId(dto.CustomerId)
            .FilterByEmail(dto.Email)
            .FilterBySent(dto.Sent)
            .FilterByRetry(dto.Retry)

            .OrderBy(dto.OrderBy);

        var count = await entities.CountAsync(cancellationToken);
        var paged = this.mapper.Map<List<NotifyDTO>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

        return ListResult<NotifyDTO>.Success(paged, count);
    }
}
