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

    public class GetNotifiesQueryHandler : IRequestHandler<GetNotifiesQuery, ListResult<Common.Entities.Notify>>
    {
        private readonly IDataAccess<Common.Entities.Notify> DataAccess;

        public GetNotifiesQueryHandler(IDataAccess<Common.Entities.Notify> DataAccess) => this.DataAccess = DataAccess;

        public async Task<ListResult<Common.Entities.Notify>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
        {
            var search = await this.DataAccess.GetAllAsync();

            return ListResult<Common.Entities.Notify>.Success(search);
        }
    }
}
