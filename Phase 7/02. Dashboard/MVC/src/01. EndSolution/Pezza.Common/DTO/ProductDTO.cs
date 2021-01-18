namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.DTO.Data;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class ProductDTO : ImageDataBase, ISearchBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public decimal? Price { get; set; }

        public bool? Special { get; set; }

        public DateTime? OfferEndDate { get; set; }

        public decimal? OfferPrice { get; set; }

        public bool? IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
