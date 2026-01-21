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

public sealed class GetNotifiesQuery : IQuery<Result<IEnumerable<NotifyDTO>>>
{
    public NotifyDTO? Data { get; set; }
}

public sealed class GetNotifiesQueryHandler(DatabaseContext databaseContext, IMapper mapper)
    : IQueryHandler<GetNotifiesQuery, Result<IEnumerable<NotifyDTO>>>
{
    public async Task<Result<IEnumerable<NotifyDTO>>> HandleAsync(GetNotifiesQuery request, CancellationToken cancellationToken)
    {
        if (request.Data == null)
        {
            return Result<IEnumerable<NotifyDTO>>.Failure("Notify search data is required");
        }

        var dto = request.Data;
        dto.OrderBy ??= "DateSent desc";

        var entities = databaseContext.Notify.Select(x => x)
            .AsNoTracking()
            .FilterByCustomerId(dto.CustomerId)
            .FilterByEmail(dto.Email)
            .FilterBySent(dto.Sent)
            .FilterByRetry(dto.Retry)

            .OrderBy(dto.OrderBy);

        var count = await entities.CountAsync(cancellationToken);
        var paged = mapper.Map<List<NotifyDTO>>(await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken));

        return Result<IEnumerable<NotifyDTO>>.Success(paged, count);
    }
}
