namespace Pezza.Portal.Models
{
    using System.Collections.Generic;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public class OrderItemModel : OrderItemDataDTO
    {
        public OrderItemModel()
        {
            this.Quantity = 0;
        }

        public List<Product> Products { set; get; }
    }
}
