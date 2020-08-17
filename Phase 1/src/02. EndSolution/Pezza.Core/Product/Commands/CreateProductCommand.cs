namespace Pezza.Core.Product.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.DataAccess.Contracts;

    public partial class CreateProductCommand : IRequest<Product>
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureData { get; set; }

        public decimal Price { get; set; }

        public bool Special { get; set; }

        public DateTime? OfferEndDate { get; set; }

        public decimal? OfferPrice { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductDataAccess dataAcess;

        public CreateProductCommandHandler(IProductDataAccess dataAcess) 
            => this.dataAcess = dataAcess;

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            => await this.dataAcess.SaveAsync(new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Special = request.Special,
                OfferEndDate = request.OfferEndDate,
                OfferPrice = request.OfferPrice,
                IsActive = request.IsActive,
                DateCreated = DateTime.Now
            });
    }
}
