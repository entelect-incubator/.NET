namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;

    public class ProductDTO : ImageDataBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public decimal? Price { get; set; }

        private decimal _price;

        public decimal _Price
        {
            get { return this.Price ?? 0; }
            set
            {
                this._price = value;
                this.Price = value;
            }
        }

        public bool? Special { get; set; }

        private bool _special;
        public bool _Special
        {
            get { return this.Special ?? false; }
            set
            {
                this._special = value;
                this.Special = value;
            }
        }

        public DateTime? OfferEndDate { get; set; }

        public decimal? OfferPrice { get; set; }

        public bool? IsActive { get; set; }

        private bool _isActive;
        public bool _IsActive
        {
            get { return this.IsActive ?? false; }
            set
            {
                this._isActive = value;
                this.IsActive = value;
            }
        }

        public DateTime DateCreated { get; set; }
    }
}
