namespace Pezza.Core.Notify.Queries;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pezza.Common.DTO;
using Pezza.Common.Models;
using Pezza.DataAccess;

public class GetNotifyQuery : IRequest<Result<NotifyDTO>>
{
    public int Id { get; set; }
}

public class GetNotifyQueryHandler : IRequestHandler<GetNotifyQuery, Result<NotifyDTO>>
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
