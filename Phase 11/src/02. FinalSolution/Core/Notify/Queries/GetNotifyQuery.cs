namespace Core.Notify.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetNotifyQuery : IQuery<Result<NotifyDTO>>
{
    public int Id { get; set; }
}

public sealed class GetNotifyQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetNotifyQuery, Result<NotifyDTO>>
{
    public async Task<Result<NotifyDTO>> Handle(GetNotifyQuery request, CancellationToken cancellationToken)
    {
        var result = (await databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken))?.ToDto();
        return Result<NotifyDTO>.Success(result);
    }
}
