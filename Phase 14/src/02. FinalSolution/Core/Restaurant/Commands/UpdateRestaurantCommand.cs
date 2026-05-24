namespace Core.Restaurant.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class UpdateRestaurantCommand : ICommand<Result<RestaurantDTO>>
{
    public RestaurantDTO Data { get; set; }
}

public sealed class UpdateRestaurantCommandHandler(DatabaseContext databaseContext) : ICommandHandler<UpdateRestaurantCommand, Result<RestaurantDTO>>
{
    public async Task<Result<RestaurantDTO>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (findEntity is null)
        {
            return Result<RestaurantDTO>.Failure("Restaurant not found");
        }

        findEntity.Name = !string.IsNullOrEmpty(dto.Name) ? dto.Name : findEntity.Name;
        findEntity.Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : findEntity.Description;
        findEntity.Address = !string.IsNullOrEmpty(dto?.Address?.Address) ? dto?.Address?.Address : findEntity.Address;
        findEntity.City = !string.IsNullOrEmpty(dto?.Address?.City) ? dto?.Address?.City : findEntity.City;
        findEntity.Province = !string.IsNullOrEmpty(dto?.Address?.Province) ? dto?.Address?.Province : findEntity.Province;
        findEntity.PostalCode = !string.IsNullOrEmpty(dto?.Address?.PostalCode) ? dto?.Address?.PostalCode : findEntity.PostalCode;
        findEntity.PictureUrl = !string.IsNullOrEmpty(dto.PictureUrl) ? dto.PictureUrl : findEntity.PictureUrl;
        findEntity.IsActive = dto.IsActive ?? findEntity.IsActive;

        databaseContext.Restaurants.Update(findEntity);

        return await CoreHelper<RestaurantDTO>.Outcome(databaseContext, cancellationToken, findEntity.ToDto(), "Error updating restaurant");
    }
}
