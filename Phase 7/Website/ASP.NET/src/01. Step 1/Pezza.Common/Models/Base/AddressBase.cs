namespace Common.Models.Base
{
    public class AddressBase : EntityBase
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }
    }
}
