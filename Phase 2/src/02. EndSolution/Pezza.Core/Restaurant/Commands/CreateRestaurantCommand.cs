namespace Pezza.Core.Restaurant.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateRestaurantCommand : IRequest<Result<Restaurant>>
    {
        public string ImageData { get; set; }

        public Restaurant Restaurant { get; set; }
    }

    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Result<Restaurant>>
    {
        private readonly IDataAccess<Common.Entities.Restaurant> dataAcess;

        public CreateRestaurantCommandHandler(IDataAccess<Common.Entities.Restaurant> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Restaurant>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            request.Restaurant.DateCreated = DateTime.Now;
            var outcome = await this.dataAcess.SaveAsync(request.Restaurant);

            return (outcome != null) ? Result<Restaurant>.Success(outcome) : Result<Restaurant>.Failure("Error adding a Restaurant");
        }
    }
}
