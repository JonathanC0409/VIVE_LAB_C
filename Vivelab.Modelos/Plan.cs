using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vivelab.Modelos;

public class Plan
{
    [Key]
    public int Code  { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public int UserCount { get; set; }

    public string Description { get; set; }

    public virtual List<Subscription>? Subscriptions { get; set; }
}
