﻿namespace Common.DTO;

using System;
using Common.Models;
using Common.Models.Base;

public class PizzaModel : EntityBase
{
    public string Name { get; set; }

    public string UnitOfMeasure { get; set; }

    public double? ValueOfMeasure { get; set; }

    public int? Quantity { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string Comment { get; set; }

    public string OrderBy { get; set; }

    public PagingArgs PagingArgs { get; set; }
}
