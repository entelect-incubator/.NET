namespace Pezza.Core.Notify.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetNotifyQuery : IRequest<Result<Common.Entities.Notify>>
    {
        public int Id { get; set; }
    }

    public class GetNotifysQueryHandler : IRequestHandler<GetNotifyQuery, Result<Common.Entities.Notify>>
    {
        private readonly IDataAccess<Common.Entities.Notify> dataAcess;

        public GetNotifysQueryHandler(IDataAccess<Common.Entities.Notify> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Notify>> Handle(GetNotifyQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAsync(request.Id);

            return Result<Common.Entities.Notify>.Success(search);
        }
    }
}
