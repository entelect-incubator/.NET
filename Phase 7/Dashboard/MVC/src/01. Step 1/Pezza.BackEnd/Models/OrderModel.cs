namespace Pezza.Portal.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Pezza.Common.DTO;

    public class OrderModel : OrderDTO
    {
        public OrderModel()
        {
            this.Amount = 0;
        }

        public List<SelectListItem> Restaurants { set; get; }

        public List<SelectListItem> Customers { set; get; }
    }
}
