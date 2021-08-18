namespace Pezza.Core.Product.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class UpdateProductCommand : IRequest<Result<ProductDTO>>
    {
        public ProductDTO Data { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDTO>>
    {
        private readonly IDataAccess<ProductDTO> dto;

        public UpdateProductCommandHandler(IDataAccess<ProductDTO> dto) => this.dto = dto;

        public async Task<Result<ProductDTO>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dto.UpdateAsync(request.Data);
            return (outcome != null) ? Result<ProductDTO>.Success(outcome) : Result<ProductDTO>.Failure("Error updating a Product");
        }
    }
}