namespace Pezza.Common.Entities;

using System;
using Pezza.Common.Models.Base;

public class Notify : EntityBase
{
    public int CustomerId { get; set; }

    public string Email { get; set; }

    public bool Sent { get; set; }

    public int Retry { get; set; }

    public DateTime DateSent { get; set; }

    public virtual Customer Customer { get; set; }
}
