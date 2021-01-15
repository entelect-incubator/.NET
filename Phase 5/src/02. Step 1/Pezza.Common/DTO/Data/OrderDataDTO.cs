namespace Pezza.Common.DTO
{
    using System;
    using System.Collections.Generic;
    using Pezza.Common.DTO.Data;

    public class OrderDataDTO : SearchBase
    {
        public CustomerDataDTO Customer { get; set; }

        public int? CustomerId { get; set; }

        public int? RestaurantId { get; set; }

        public decimal? Amount { get; set; }

        public bool? Completed { get; set; }

        public virtual ICollection<OrderItemDataDTO> OrderItems { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
