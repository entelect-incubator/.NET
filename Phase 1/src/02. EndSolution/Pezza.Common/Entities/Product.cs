namespace Pezza.Common.Entities
{
    using System;
    using System.Collections.Generic;

    public partial class Product
    {
        public Product()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public decimal Price { get; set; }

        public bool Special { get; set; }

        public DateTime? OfferEndDate { get; set; }

        public decimal? OfferPrice { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
