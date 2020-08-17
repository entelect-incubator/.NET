namespace Pezza.Core.Restaurant.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateRestaurantCommand : IRequest<Result<Common.Entities.Restaurant>>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureData { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Result<Common.Entities.Restaurant>>
    {
        private readonly IRestaurantDataAccess dataAcess;

        public UpdateRestaurantCommandHandler(IRestaurantDataAccess dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Restaurant>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(new RestaurantSearchModel
            {
                Name = request.Name
            });
            var findEntity = search.FirstOrDefault();

            if (!string.IsNullOrEmpty(request.Name))
            {
                findEntity.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                findEntity.Description = request.Description;
            }

            if (!string.IsNullOrEmpty(request.Address))
            {
                findEntity.Address = request.Address;
            }

            if (!string.IsNullOrEmpty(request.City))
            {
                findEntity.City = request.City;
            }

            if (!string.IsNullOrEmpty(request.Province))
            {
                findEntity.Province = request.Province;
            }

            if (!string.IsNullOrEmpty(request.PostalCode))
            {
                findEntity.PostalCode = request.PostalCode;
            }

            if (request.IsActive.HasValue)
            {
                findEntity.IsActive = request.IsActive.Value;
            }

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Restaurant>.Success(outcome) : Result<Common.Entities.Restaurant>.Failure("Error updating a Restaurant");
        }
    }
}