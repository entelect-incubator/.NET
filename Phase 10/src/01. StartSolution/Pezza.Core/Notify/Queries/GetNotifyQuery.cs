namespace Core.Notify.Queries;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.DTO;
using Pezza.Pezza.Common.Models;
using DataAccess;

public sealed class GetNotifyQuery : ICommand<Result<NotifyDTO>>
{
    public int Id { get; set; }
}

public sealed class GetNotifyQueryHandler : ICommandHandler<GetNotifyQuery, Result<NotifyDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetNotifyQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<NotifyDTO>> Handle(GetNotifyQuery request, CancellationToken cancellationToken)
    {
        var result = this.mapper.Map<NotifyDTO>(await this.databaseContext.Notify.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        return Result<NotifyDTO>.Success(result);
    }
}

