namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class RestaurantDTO : ImageDataBase, Data.ISearchBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public AddressBase Address { get; set; }

        public bool? IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
