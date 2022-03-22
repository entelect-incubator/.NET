namespace Pezza.Common.Entities
{
    using System;

    public class Restaurant : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
