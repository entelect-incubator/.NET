namespace Pezza.Common.Entities
{
    using System;

    public class Notify
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Email { get; set; }

        public bool Sent { get; set; }

        public int Retry { get; set; }

        public DateTime DateSent { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
