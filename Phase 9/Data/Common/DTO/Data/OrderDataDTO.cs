namespace Pezza.Common.DTO
{
    using System.Collections.Generic;

    public class OrderDataDTO
    {
        public CustomerDataDTO Customer { get; set; }

        public int? CustomerId { get; set; }

        public int? RestaurantId { get; set; }

        public decimal? Amount { get; set; }
        
        public bool? Completed { get; set; }

        public virtual ICollection<OrderItemDataDTO> OrderItems { get; set; }

    }
}
