namespace Pezza.Core.Restaurant.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Pezza.Common.DTO;
using Pezza.Common.Entities;
using Pezza.Common.Models;
using Pezza.Core.Helpers;
using Pezza.DataAccess;

public class CreateRestaurantCommand : IRequest<Result<RestaurantDTO>>
{
    public RestaurantDTO Data { get; set; }
}

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Result<RestaurantDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public CreateRestaurantCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<RestaurantDTO>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var entity = this.mapper.Map<Restaurant>(request.Data);
        this.databaseContext.Restaurants.Add(entity);

        return await CoreHelper<RestaurantDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, entity, "Error creating a restaurant");
    }
}
