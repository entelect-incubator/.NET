namespace Pezza.Core.Notify.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class GetNotifyQuery : IRequest<ListResult<Common.Entities.Notify>>
    {
        public NotifySearchModel NotifySearchModel { get; set; }
    }

    public class GetNotifysQueryHandler : IRequestHandler<GetNotifyQuery, ListResult<Common.Entities.Notify>>
    {
        private readonly INotifyDataAccess dataAcess;

        public GetNotifysQueryHandler(INotifyDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Notify>> Handle(GetNotifyQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.NotifySearchModel);

            return ListResult<Common.Entities.Notify>.Success(search);
        }
    }
}
