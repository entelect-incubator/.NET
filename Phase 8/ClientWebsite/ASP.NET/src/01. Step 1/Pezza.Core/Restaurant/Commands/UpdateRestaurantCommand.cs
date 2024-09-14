namespace Core.Restaurant.Commands
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

    public class UpdateRestaurantCommand : IRequest<Result<RestaurantDTO>>
    {
        public RestaurantDTO Data { get; set; }
    }

    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Result<RestaurantDTO>>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public UpdateRestaurantCommandHandler(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<Result<RestaurantDTO>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var findEntity = await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
            if (findEntity == null)
            {
                return null;
            }

            findEntity.Name = !string.IsNullOrEmpty(dto.Name) ? dto.Name : findEntity.Name;
            findEntity.Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : findEntity.Description;
            findEntity.Address = !string.IsNullOrEmpty(dto?.Address?.Address) ? dto?.Address?.Address : findEntity.Address;
            findEntity.City = !string.IsNullOrEmpty(dto?.Address?.City) ? dto?.Address?.City : findEntity.City;
            findEntity.Province = !string.IsNullOrEmpty(dto?.Address?.Province) ? dto?.Address?.Province : findEntity.Province;
            findEntity.PostalCode = !string.IsNullOrEmpty(dto?.Address?.PostalCode) ? dto?.Address?.PostalCode : findEntity.PostalCode;
            findEntity.PictureUrl = !string.IsNullOrEmpty(dto.PictureUrl) ? dto.PictureUrl : findEntity.PictureUrl;
            findEntity.IsActive = dto.IsActive ?? findEntity.IsActive;

            var outcome = this.databaseContext.Restaurants.Update(findEntity);

            return await CoreHelper<RestaurantDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, findEntity, "Error updating restaurant");
        }
    }
}