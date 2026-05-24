namespace Core.Notify.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Extensions;
using Common.Filters;
using Common.Mapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetNotifiesQuery : IQuery<Result<IEnumerable<NotifyDTO>>>
{
    public NotifyDTO Data { get; set; }
}

public sealed class GetNotifiesQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetNotifiesQuery, Result<IEnumerable<NotifyDTO>>>
{
    public async Task<Result<IEnumerable<NotifyDTO>>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data ?? new NotifyDTO();
        if (string.IsNullOrEmpty(dto.OrderBy))
        {
            dto.OrderBy = "DateSent desc";
        }

        var entities = databaseContext.Notify.Select(x => x)
            .AsNoTracking()
            .FilterByCustomerId(dto.CustomerId)
            .FilterByEmail(dto.Email)
            .FilterBySent(dto.Sent)
            .FilterByRetry(dto.Retry)

            .OrderBy(dto.OrderBy);

        var count = await entities.CountAsync(cancellationToken);
        var paged = (await entities.ApplyPaging(dto.PagingArgs).OrderBy(dto.OrderBy).ToListAsync(cancellationToken)).Select(x => x.ToDto()).ToList();

        return Result<IEnumerable<NotifyDTO>>.Success(paged, count);
    }
}
