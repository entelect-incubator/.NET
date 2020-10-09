namespace Pezza.Core.Notify.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetNotifiesQuery : IRequest<ListResult<Common.Entities.Notify>>
    {
    }

    public class GetNotifiessQueryHandler : IRequestHandler<GetNotifiesQuery, ListResult<Common.Entities.Notify>>
    {
        private readonly IDataAccess<Common.Entities.Notify> dataAcess;

        public GetNotifiessQueryHandler(IDataAccess<Common.Entities.Notify> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Notify>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync();

            return ListResult<Common.Entities.Notify>.Success(search);
        }
    }
}
