namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Models;

    public class NotifyDTO : SearchBase
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public string Email { get; set; }

        public bool? Sent { get; set; }

        public int? Retry { get; set; }

        public DateTime? DateSent { get; set; } = DateTime.Now;
    }
}
