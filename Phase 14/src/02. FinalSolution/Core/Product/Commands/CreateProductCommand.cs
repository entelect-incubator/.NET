namespace Core.Product.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;

public sealed class CreateProductCommand : ICommand<Result<ProductDTO>>
{
    public ProductDTO Data { get; set; }
}

public sealed class CreateProductCommandHandler(DatabaseContext databaseContext) : ICommandHandler<CreateProductCommand, Result<ProductDTO>>
{
    public async Task<Result<ProductDTO>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Data.ToEntity();
        databaseContext.Products.Add(entity);
        var outcome = await CoreHelper<ProductDTO>.Outcome(databaseContext, cancellationToken, request.Data, "Error creating a product");
        if (outcome.Succeeded)
        {
            request.Data.Id = entity.Id;
        }

        return outcome;
    }
}
