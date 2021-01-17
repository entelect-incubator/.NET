namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;

    public class ProductDTO : ImageDataBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public decimal? Price
        {
            get => this.Price ?? 0;
            set => this.Price = value;
        }

        public bool? Special
        {
            get => this.Special ?? false;
            set => this.Special = value;
        }

        public DateTime? OfferEndDate { get; set; }

        public decimal? OfferPrice { get; set; }

        public bool? IsActive
        {
            get => this.IsActive ?? false;
            set => this.IsActive = value;
        }

        public DateTime DateCreated { get; set; }
    }
}
