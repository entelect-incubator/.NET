namespace Pezza.Common.DTO
{
    public class NotifyDataDTO
    {
        public int? CustomerId { get; set; }

        public string Email { get; set; }

        public bool? Sent { get; set; }

        public int? Retry { get; set; }
    }
}
