namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.DTO.Data;

    public class NotifyDataDTO : SearchBase
    {
        public int? CustomerId { get; set; }

        public string Email { get; set; }

        public bool? Sent { get; set; }

        public int? Retry { get; set; }

        public DateTime? DateSent { get; set; }
    }
}
