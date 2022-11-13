namespace Pezza.Portal.Models;

using System.Collections.Generic;
using Pezza.Common.DTO;
using Pezza.Common.Entities;

public class OrderItemModel : OrderItemDTO
{
    public OrderItemModel()
    {
        this.Quantity = 0;
    }

    public List<ProductModel> Products { set; get; }
}
