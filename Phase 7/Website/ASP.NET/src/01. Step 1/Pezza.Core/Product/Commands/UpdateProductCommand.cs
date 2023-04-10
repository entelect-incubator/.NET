namespace Core.Product.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Common.DTO;
    using Common.Models;
    using Core.Helpers;
    using DataAccess;

    public class UpdateProductCommand : IRequest<Result<ProductDTO>>
    {
        public ProductDTO Data { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDTO>>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public UpdateProductCommandHandler(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<Result<ProductDTO>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var findEntity = await this.databaseContext.Products.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken: cancellationToken);
            if (findEntity == null)
            {
                return null;
            }

            findEntity.Name = !string.IsNullOrEmpty(dto.Name) ? dto.Name : findEntity.Name;
            findEntity.Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : findEntity.Description;
            findEntity.PictureUrl = !string.IsNullOrEmpty(dto.PictureUrl) ? dto.PictureUrl : findEntity.PictureUrl;
            findEntity.Price = dto.Price ?? findEntity.Price;
            findEntity.Special = dto.Special ?? findEntity.Special;
            findEntity.OfferEndDate = dto.OfferEndDate ?? findEntity.OfferEndDate;
            findEntity.OfferPrice = dto.OfferPrice ?? findEntity.OfferPrice;
            findEntity.IsActive = dto.IsActive ?? findEntity.IsActive;

            var outcome = this.databaseContext.Products.Update(findEntity);

            return await CoreHelper<ProductDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, findEntity, "Error updating product");
        }
    }
}