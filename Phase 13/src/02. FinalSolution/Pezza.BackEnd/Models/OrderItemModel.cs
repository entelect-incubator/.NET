namespace Portal.Models;

using System.Collections.Generic;
using Common.DTO;
using Common.Entities;

public class OrderItemModel : OrderItemDTO
{
    public OrderItemModel()
    {
        this.Quantity = 0;
    }

    public List<ProductModel> Products { set; get; }
}
