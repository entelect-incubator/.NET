﻿namespace Common.DTO;

using System;
using Common.Models;
using Common.Models.Base;

public class NotifyDTO : EntityBase
{
    public int? CustomerId { get; set; }

    public string Email { get; set; }

    public bool? Sent { get; set; }

    public int? Retry { get; set; }

    public DateTime? DateSent { get; set; } = DateTime.Now;

    public string OrderBy { get; set; }

    public PagingArgs PagingArgs { get; set; }
}
