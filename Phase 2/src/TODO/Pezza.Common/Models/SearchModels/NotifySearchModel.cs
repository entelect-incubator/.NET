namespace Pezza.Common.Models.SearchModels
{
    using System;

    public class NotifySearchModel
    {
        public int CustomerId { get; set; }

        public string Email { get; set; }

        public bool? Sent { get; set; }

        public int? Retry { get; set; }

        public DateTime? DateSent { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}