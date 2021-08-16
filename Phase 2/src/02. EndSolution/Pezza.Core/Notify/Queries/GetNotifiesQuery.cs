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
    }

    public class GetNotifiesQueryHandler : IRequestHandler<GetNotifiesQuery, ListResult<NotifyDTO>>
    {
        private readonly IDataAccess<NotifyDTO> dataAcess;

        public GetNotifiesQueryHandler(IDataAccess<NotifyDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<NotifyDTO>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync();
            return ListResult<NotifyDTO>.Success(search, search.Count);
        }
    }
}
