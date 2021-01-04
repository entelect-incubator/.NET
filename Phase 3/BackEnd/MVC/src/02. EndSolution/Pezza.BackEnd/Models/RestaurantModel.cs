namespace Pezza.Portal.Models
{
    using Microsoft.AspNetCore.Http;
    using Pezza.Common.DTO;

    public class RestaurantModel : RestaurantDataDTO
    {
        public int Id { set; get; }

        public IFormFile Image { set; get; }
    }
}
