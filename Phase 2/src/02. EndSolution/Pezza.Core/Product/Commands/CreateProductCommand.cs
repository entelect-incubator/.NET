namespace Pezza.Core.Product.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateProductCommand : IRequest<Result<Product>>
    {
        public ProductDataDTO Data { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Product>>
    {
        private readonly IDataAccess<Product> dataAcess;

        public CreateProductCommandHandler(IDataAccess<Product> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAcess.SaveAsync(request.Data.Map());

            return (outcome != null) ? Result<Product>.Success(outcome) : Result<Product>.Failure("Error adding a Product");
        }
    }
}
