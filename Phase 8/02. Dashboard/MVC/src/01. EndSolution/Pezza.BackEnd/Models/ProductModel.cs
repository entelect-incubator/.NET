namespace Pezza.Portal.Models
{
    using Microsoft.AspNetCore.Http;
    using Pezza.Common.DTO;

    public class ProductModel : ProductDataDTO
    {
        public int Id { set; get; }

        public IFormFile Image { set; get; }

        public bool HasOffer { set; get; } = false;
    }
}
