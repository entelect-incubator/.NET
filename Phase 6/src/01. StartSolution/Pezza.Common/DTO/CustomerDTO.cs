namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class CustomerDTO : SearchBase
    {
        public CustomerDTO()
        {
            this.Address = new AddressBase();
            this.DateCreated = DateTime.Now;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public AddressBase Address { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
