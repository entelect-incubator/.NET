namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.DTO.Data;
    using Pezza.Common.Entities;

    public class CustomerDataDTO : SearchBase
    {
        public CustomerDataDTO()
            :base()
        {                
        }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public AddressBase Address { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
