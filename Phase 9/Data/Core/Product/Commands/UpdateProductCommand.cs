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
        private readonly IDataAccess<Common.Entities.Product> DataAccess;

        public UpdateProductCommandHandler(IDataAccess<Common.Entities.Product> DataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<Common.Entities.Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAccess.GetAsync(request.Id);
            findEntity.Name = !string.IsNullOrEmpty(request.Data?.Name) ? request.Data?.Name : findEntity.Name;
            findEntity.Description = !string.IsNullOrEmpty(request.Data?.Description) ? request.Data?.Description : findEntity.Description;
            findEntity.PictureUrl = !string.IsNullOrEmpty(request.Data?.PictureUrl) ? request.Data?.PictureUrl : findEntity.PictureUrl;
            findEntity.Price = request.Data.Price ?? findEntity.Price;
            findEntity.Special = request.Data.Special ?? findEntity.Special;
            findEntity.OfferEndDate = request.Data.OfferEndDate ?? findEntity.OfferEndDate;
            findEntity.OfferPrice = request.Data.OfferPrice ?? findEntity.OfferPrice;
            findEntity.IsActive = request.Data.IsActive ?? findEntity.IsActive;
            var outcome = await this.dataAccess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Product>.Success(outcome) : Result<Common.Entities.Product>.Failure("Error updating a Product");
        }
    }
}