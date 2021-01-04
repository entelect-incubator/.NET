namespace Pezza.Common.Entities
{
    using System;
    using System.Collections.Generic;

    public class Restaurant
    {
        public Restaurant() => this.Orders = new HashSet<Order>();

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
