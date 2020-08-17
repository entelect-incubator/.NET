namespace Pezza.Common.Entities
{
	using System;
	using System.Collections.Generic;

    public partial class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
