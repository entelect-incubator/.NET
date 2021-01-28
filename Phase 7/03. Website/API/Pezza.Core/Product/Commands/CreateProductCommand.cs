namespace Pezza.Core.Product.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateProductCommand : IRequest<Result<ProductDTO>>
    {
        public ProductDTO Data { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDTO>>
    {
        private readonly IDataAccess<ProductDTO> dataAcess;

        public CreateProductCommandHandler(IDataAccess<ProductDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<ProductDTO>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAcess.SaveAsync(request.Data);
            return (outcome != null) ? Result<ProductDTO>.Success(outcome) : Result<ProductDTO>.Failure("Error adding a Product");
        }
    }
}
