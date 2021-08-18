namespace Pezza.Core.Notify.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetNotifiesQuery : IRequest<ListResult<NotifyDTO>>
    {
        public NotifyDTO dto;
    }

    public class GetNotifiesQueryHandler : IRequestHandler<GetNotifiesQuery, ListResult<NotifyDTO>>
    {
        private readonly IDataAccess<NotifyDTO> dto;

        public GetNotifiesQueryHandler(IDataAccess<NotifyDTO> dto) => this.dto = dto;

        public async Task<ListResult<NotifyDTO>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dto.GetAllAsync(request.dto);
            return search;
        }
    }
}
