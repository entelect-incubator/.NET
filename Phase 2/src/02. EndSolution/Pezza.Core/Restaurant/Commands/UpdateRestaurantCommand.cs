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
        private readonly IDataAccess<Common.Entities.Restaurant> dataAcess;

        public UpdateRestaurantCommandHandler(IDataAccess<Common.Entities.Restaurant> dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Restaurant>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
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

            if (!string.IsNullOrEmpty(request.Data?.Address?.Address))
            {
                findEntity.Address = request.Data?.Address?.Address;
            }

            if (!string.IsNullOrEmpty(request.Data?.Address?.City))
            {
                findEntity.City = request.Data?.Address?.City;
            }

            if (!string.IsNullOrEmpty(request.Data?.Address?.Province))
            {
                findEntity.Province = request.Data?.Address?.Province;
            }

            if (!string.IsNullOrEmpty(request.Data?.Address?.ZipCode))
            {
                findEntity.PostalCode = request.Data?.Address?.ZipCode;
            }

            if (!string.IsNullOrEmpty(request.Data?.PictureUrl))
            {
                findEntity.PictureUrl = request.Data?.PictureUrl;
            }

            if (request.Data.IsActive.HasValue)
            {
                findEntity.IsActive = request.Data.IsActive.Value;
            }

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Restaurant>.Success(outcome) : Result<Common.Entities.Restaurant>.Failure("Error updating a Restaurant");
        }
    }
}