namespace Pezza.Common.DTO
{
    using Pezza.Common.Entities;

    public class RestaurantDataDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public AddressBase Address { get; set; }
    }
}
