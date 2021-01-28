namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class NotifyDTO : Entity, Data.ISearchBase
    {
        public int? CustomerId { get; set; }

        public virtual CustomerDTO Customer { get; set; }

        public string Email { get; set; }

        public bool? Sent { get; set; }

        public int? Retry { get; set; }

        public DateTime? DateSent { get; set; } = DateTime.Now;

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
