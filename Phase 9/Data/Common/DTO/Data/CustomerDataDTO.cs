namespace Pezza.Common.DTO
{
    using Pezza.Common.Entities;

    public class CustomerDataDTO
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public AddressBase Address { get; set; }
    }
}
