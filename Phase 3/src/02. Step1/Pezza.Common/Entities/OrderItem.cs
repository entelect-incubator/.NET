namespace Pezza.Common.Entities;

using Pezza.Common.Models.Base;

public class OrderItem : EntityBase
{

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public Product Product { get; set; }
}
