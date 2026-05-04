namespace Common.Entities;

using System;
using System.Collections.Generic;
using Common.Models.Base;

public class Customer : EntityBase
{
    public Customer() => this.Orders = new HashSet<Order>();

    public string Name { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Province { get; set; }

    public string PostalCode { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string ContactPerson { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
