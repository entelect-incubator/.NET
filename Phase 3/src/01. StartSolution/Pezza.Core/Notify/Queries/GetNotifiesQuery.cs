namespace Pezza.Core.Notify.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess;

    public class GetNotifiesQuery : IRequest<ListResult<NotifyDTO>>
    {
        public int Id { get; set; }
    }

    public class GetNotifiesQueryHandler : IRequestHandler<GetNotifiesQuery, ListResult<NotifyDTO>>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public GetNotifiesQueryHandler(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<ListResult<NotifyDTO>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
        {
            var entities = this.databaseContext.Notify.Select(x => x).AsNoTracking();

            var count = entities.Count();
            var paged = this.mapper.Map<List<NotifyDTO>>(await entities.ToListAsync(cancellationToken));

            return ListResult<NotifyDTO>.Success(paged, count);
        }
    }
}
