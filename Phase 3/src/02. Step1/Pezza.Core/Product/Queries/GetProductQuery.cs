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
        private readonly IDataAccess<Common.Entities.Product> dataAcess;

        public GetProductQueryHandler(IDataAccess<Common.Entities.Product> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAsync(request.Id);

            return Result<Common.Entities.Product>.Success(search);
        }
    }
}
