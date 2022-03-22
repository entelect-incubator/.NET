namespace Pezza.Core.Notify.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetNotifyQuery : IRequest<Result<NotifyDTO>>
    {
        public int Id { get; set; }
    }

    public class GetNotifyQueryHandler : IRequestHandler<GetNotifyQuery, Result<NotifyDTO>>
    {
        private readonly IDataAccess<NotifyDTO> dataAccess;

        public GetNotifyQueryHandler(IDataAccess<NotifyDTO> dataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<NotifyDTO>> Handle(GetNotifyQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAccess.GetAsync(request.Id);
            return Result<NotifyDTO>.Success(search);
        }
    }
}
