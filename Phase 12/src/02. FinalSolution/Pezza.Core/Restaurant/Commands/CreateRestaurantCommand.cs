namespace Core.Restaurant.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Common.DTO;
using Common.Entities;
using Common.Models;
using Core.Helpers;
using DataAccess;

public sealed class CreateRestaurantCommand : ICommand<Result<RestaurantDTO>>
{
    public RestaurantDTO Data { get; set; }
}

public sealed class CreateRestaurantCommandHandler : ICommandHandler<CreateRestaurantCommand, Result<RestaurantDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public CreateRestaurantCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<RestaurantDTO>> HandleAsync(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var entity = this.mapper.Map<Restaurant>(request.Data);
        this.databaseContext.Restaurants.Add(entity);

        return await CoreHelper<RestaurantDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, entity, "Error creating a restaurant");
    }
}
