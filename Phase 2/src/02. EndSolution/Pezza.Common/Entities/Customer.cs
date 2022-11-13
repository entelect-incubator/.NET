namespace Pezza.Common.Entities;

using System;
using System.Collections.Generic;
using Pezza.Common.Models.Base;

public class Customer : AddressBase
{
    public string Name { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string ContactPerson { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual ICollection<Notify> Notifies { get; set; }
}
