namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.DTO.Data;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class NotifyDTO : Entity, ISearchBase
    {
        public int? CustomerId { get; set; }

        public string Email { get; set; }

        public bool? Sent { get; set; }

        public int? Retry { get; set; }

        public DateTime? DateSent { get; set; } = DateTime.Now;

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
