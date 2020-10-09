namespace Pezza.Core.Product.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateProductCommand : IRequest<Result<Product>>
    {
        public string ImageData { get; set; }

        public Product Product { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Product>>
    {
        private readonly IDataAccess<Common.Entities.Product> dataAcess;

        public CreateProductCommandHandler(IDataAccess<Common.Entities.Product> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            request.Product.DateCreated = DateTime.Now;
            var outcome = await this.dataAcess.SaveAsync(request.Product);

            return (outcome != null) ? Result<Product>.Success(outcome) : Result<Product>.Failure("Error adding a Product");
        }
    }
}
