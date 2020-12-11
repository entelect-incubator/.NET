namespace Pezza.Core.Product.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateProductCommand : IRequest<Result<Common.Entities.Product>>
    {
        public int Id { get; set; }

        public ProductDataDTO Data { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<Common.Entities.Product>>
    {
        private readonly IDataAccess<Common.Entities.Product> dataAcess;

        public UpdateProductCommandHandler(IDataAccess<Common.Entities.Product> dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);

            if (!string.IsNullOrEmpty(request.Data?.Name))
            {
                findEntity.Name = request.Data?.Name;
            }

            if (!string.IsNullOrEmpty(request.Data?.Description))
            {
                findEntity.Description = request.Data?.Description;
            }

            if (!string.IsNullOrEmpty(request.Data?.PictureUrl))
            {
                findEntity.PictureUrl = request.Data?.PictureUrl;
            }

            if (request.Data.Price.HasValue)
            {
                findEntity.Price = request.Data.Price.Value;
            }

            if (request.Data.Special.HasValue)
            {
                findEntity.Special = request.Data.Special.Value;
            }

            if (request.Data.OfferEndDate.HasValue)
            {
                findEntity.OfferEndDate = request.Data.OfferEndDate;
            }

            if (request.Data.OfferPrice.HasValue)
            {
                findEntity.OfferPrice = request.Data.OfferPrice;
            }

            if (request.Data.IsActive.HasValue)
            {
                findEntity.IsActive = request.Data.IsActive.Value;
            }

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Product>.Success(outcome) : Result<Common.Entities.Product>.Failure("Error updating a Product");
        }
    }
}