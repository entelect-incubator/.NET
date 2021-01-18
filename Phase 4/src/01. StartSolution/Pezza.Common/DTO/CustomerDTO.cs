namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class CustomerDTO : Entity, Data.ISearchBase
    {
        public CustomerDTO()
        {
            this.Address = new AddressBase();
        }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public AddressBase Address { get; set; }

        public DateTime? DateCreated { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
