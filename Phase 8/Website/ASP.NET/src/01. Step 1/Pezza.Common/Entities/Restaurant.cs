namespace Common.Entities
{
    using System;
    using Common.Models.Base;

    public class Restaurant : AddressBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
