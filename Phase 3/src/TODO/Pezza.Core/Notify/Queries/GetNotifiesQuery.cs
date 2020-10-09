namespace Pezza.Core.Notify.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class GetNotifiesQuery : IRequest<ListResult<Common.Entities.Notify>>
    {
        public NotifySearchModel NotifySearchModel { get; set; }
    }

    public class GetNotifiessQueryHandler : IRequestHandler<GetNotifiesQuery, ListResult<Common.Entities.Notify>>
    {
        private readonly INotifyDataAccess dataAcess;

        public GetNotifiessQueryHandler(INotifyDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Notify>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.NotifySearchModel);

            return ListResult<Common.Entities.Notify>.Success(search);
        }
    }
}
