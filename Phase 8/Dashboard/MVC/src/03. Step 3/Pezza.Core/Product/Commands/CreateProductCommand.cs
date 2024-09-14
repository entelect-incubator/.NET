namespace Core.Product.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Common.DTO;
using Common.Entities;
using Common.Models;
using Core.Helpers;
using DataAccess;

public class CreateProductCommand : IRequest<Result<ProductDTO>>
{
    public ProductDTO Data { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public CreateProductCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<ProductDTO>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = this.mapper.Map<Product>(request.Data);
        this.databaseContext.Products.Add(entity);

        return await CoreHelper<ProductDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, entity, "Error creating a product");
    }
}
