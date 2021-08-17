namespace Pezza.Core.Product.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetProductQuery : IRequest<Result<Common.Entities.Product>>
    {
        public int Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<Common.Entities.Product>>
    {
        private readonly IDataAccess<Common.Entities.Product> DataAccess;

        public GetProductQueryHandler(IDataAccess<Common.Entities.Product> DataAccess) => this.DataAccess = DataAccess;

        public async Task<Result<Common.Entities.Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var search = await this.DataAccess.GetAsync(request.Id);

            return Result<Common.Entities.Product>.Success(search);
        }
    }
}
