namespace Pezza.Common.DTO
{
    using System;

    public class NotifyDTO
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public string Email { get; set; }

        public bool? Sent { get; set; }

        public int? Retry { get; set; }

        public DateTime? DateSent { get; set; } = DateTime.Now;
    }
}
