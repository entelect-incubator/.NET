namespace Pezza.Common.DTO
{
    using System;
    using System.Collections.Generic;
    using Pezza.Common.Entities;

    public class CustomerDTO : Entity
    {
        public CustomerDTO() => this.Orders = new HashSet<OrderDTO>();

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<OrderDTO> Orders { get; set; }
    }
}
