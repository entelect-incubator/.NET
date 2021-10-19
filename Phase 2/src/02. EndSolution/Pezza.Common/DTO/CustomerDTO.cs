namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Models.Base;

    public class CustomerDTO : EntityBase
    {
        public CustomerDTO()
        {
            this.Address = new AddressBase();
            this.DateCreated = DateTime.Now;
        }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public AddressBase Address { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
