namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class RestaurantDataDTO : ImageDataBase
    {
        public RestaurantDataDTO() => this.Address = new AddressBase();

        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public AddressBase Address { get; set; }

        public DateTime? DateCreated { get; set; }

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

        public bool BustCache { get; set; } = false;
    }
}
