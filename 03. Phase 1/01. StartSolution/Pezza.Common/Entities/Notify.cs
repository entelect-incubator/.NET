namespace Pezza.Common.Entities
{
	using System;
	using System.Collections.Generic;

    public partial class Notify
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Email { get; set; }

        public bool Sent { get; set; }

        public int Retry { get; set; }

        public DateTime DateSent { get; set; }
    }
}
