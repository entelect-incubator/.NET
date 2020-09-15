namespace Pezza.Core.Product.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateProductCommand : IRequest<Result<Common.Entities.Product>>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageData { get; set; }
        
        public string PictureUrl { get; set; }

        public decimal? Price { get; set; }

        public bool? Special { get; set; }

        public DateTime? OfferEndDate { get; set; }

        public decimal? OfferPrice { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<Common.Entities.Product>>
    {
        private readonly IProductDataAccess dataAcess;

        public UpdateProductCommandHandler(IProductDataAccess dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);

            if (!string.IsNullOrEmpty(request.Name))
            {
                findEntity.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                findEntity.Description = request.Description;
            }

            if (!string.IsNullOrEmpty(request.PictureUrl))
            {
                findEntity.PictureUrl = request.PictureUrl;
            }

            if (request.Price.HasValue)
            {
                findEntity.Price = request.Price.Value;
            }

            if (request.Special.HasValue)
            {
                findEntity.Special = request.Special.Value;
            }

            if (request.OfferEndDate.HasValue)
            {
                findEntity.OfferEndDate = request.OfferEndDate;
            }

            if (request.OfferPrice.HasValue)
            {
                findEntity.OfferPrice = request.OfferPrice;
            }

            if (request.IsActive.HasValue)
            {
                findEntity.IsActive = request.IsActive.Value;
            }

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Product>.Success(outcome) : Result<Common.Entities.Product>.Failure("Error updating a Product");
        }
    }
}