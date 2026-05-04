namespace Core.Product.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class UpdateProductCommand : ICommand<Result<ProductDTO>>
{
    public ProductDTO Data { get; set; }
}

public sealed class UpdateProductCommandHandler(DatabaseContext databaseContext) : ICommandHandler<UpdateProductCommand, Result<ProductDTO>>
{
    public async Task<Result<ProductDTO>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await databaseContext.Products.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken: cancellationToken);
        if (findEntity is null)
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

        databaseContext.Products.Update(findEntity);

        return await CoreHelper<ProductDTO>.Outcome(databaseContext, cancellationToken, findEntity.ToDto(), "Error updating product");
    }
}