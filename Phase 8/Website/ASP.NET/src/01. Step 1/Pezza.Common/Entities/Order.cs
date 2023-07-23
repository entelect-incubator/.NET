namespace Common.Entities
{
    using System;
    using System.Collections.Generic;
    using Common.Models.Base;

    public class Order : EntityBase
    {
        public Order() => this.OrderItems = new HashSet<OrderItem>();

        public int CustomerId { get; set; }

        public int RestaurantId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateCreated { get; set; }

        public bool Completed { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
