namespace Pezza.Common.Entities
{
	using System;
	using System.Collections.Generic;

    public partial class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int RestaurantId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateCreated { get; set; }
		
        public virtual Customer Customer { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
