namespace Pezza.Portal.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Pezza.Common.DTO;

    public class OrderModel : OrderDataDTO
    {
        public OrderModel()
        {
            this.Amount = 0;
        }

        public int Id { set; get; }

        public List<SelectListItem> Restaurants { set; get; }

        public List<SelectListItem> Customers { set; get; }
    }
}
