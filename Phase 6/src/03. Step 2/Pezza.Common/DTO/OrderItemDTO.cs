namespace Pezza.Common.DTO;

using Pezza.Common.Models;
using Pezza.Common.Models.Base;

public class OrderItemDTO : EntityBase
{
    public int? OrderId { get; set; }

    public int? Quantity { get; set; }

    public int? ProductId { get; set; }

    public ProductDTO Product { get; set; }

    public string OrderBy { get; set; }

    public PagingArgs PagingArgs { get; set; }
}
