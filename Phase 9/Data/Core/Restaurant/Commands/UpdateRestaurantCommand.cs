namespace Pezza.Core.Restaurant.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateRestaurantCommand : IRequest<Result<Common.Entities.Restaurant>>
    {
        public int Id { get; set; }

        public RestaurantDataDTO Data { get; set; }
    }

    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Result<Common.Entities.Restaurant>>
    {
        private readonly IDataAccess<Common.Entities.Restaurant> DataAccess;

        public UpdateRestaurantCommandHandler(IDataAccess<Common.Entities.Restaurant> DataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<Common.Entities.Restaurant>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAccess.GetAsync(request.Id);
            findEntity.Name = !string.IsNullOrEmpty(request.Data?.Name) ? request.Data?.Name : findEntity.Name;
            findEntity.Description = !string.IsNullOrEmpty(request.Data?.Description) ? request.Data?.Description : findEntity.Description;
            findEntity.Address = !string.IsNullOrEmpty(request.Data?.Address?.Address) ? request.Data?.Address?.Address : findEntity.Address;
            findEntity.City = !string.IsNullOrEmpty(request.Data?.Address?.City) ? request.Data?.Address?.City : findEntity.City;
            findEntity.Province = !string.IsNullOrEmpty(request.Data?.Address?.Province) ? request.Data?.Address?.Province : findEntity.Province;
            findEntity.PostalCode = !string.IsNullOrEmpty(request.Data?.Address?.PostalCode) ? request.Data?.Address?.PostalCode : findEntity.PostalCode;
            findEntity.PictureUrl = !string.IsNullOrEmpty(request.Data?.PictureUrl) ? request.Data?.PictureUrl : findEntity.PictureUrl;
            findEntity.IsActive = request.Data.IsActive ?? findEntity.IsActive;
            var outcome = await this.dataAccess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Restaurant>.Success(outcome) : Result<Common.Entities.Restaurant>.Failure("Error updating a Restaurant");
        }
    }
}