namespace Pezza.Core.Restaurant.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.DataAccess.Contracts;

    public partial class CreateRestaurantCommand : IRequest<Restaurant>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureData { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Restaurant>
    {
        private readonly IRestaurantDataAccess dataAcess;

        public CreateRestaurantCommandHandler(IRestaurantDataAccess dataAcess) 
            => this.dataAcess = dataAcess;

        public async Task<Restaurant> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
            => await this.dataAcess.SaveAsync(new Restaurant
            {
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                City = request.City,
                Province = request.Province,
                PostalCode = request.PostalCode,
                IsActive = request.IsActive,
                DateCreated = DateTime.Now
            });
    }
}
