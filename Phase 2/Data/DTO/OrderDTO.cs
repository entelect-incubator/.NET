namespace Pezza.Common.DTO
{
    using System;
    using System.Collections.Generic;

    public class OrderDTO
    {
        public OrderDTO() => this.OrderItems = new HashSet<OrderItemDTO>();

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int RestaurantId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual CustomerDTO Customer { get; set; }
        
        public virtual RestaurantDTO Restaurant { get; set; }

        public virtual ICollection<OrderItemDTO> OrderItems { get; set; }

    }
}
