using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class Subscription
{
    [Key]
    public int Code { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; }

    public int PlanCode { get; set; }

    public int UserCode { get; set; }

    public virtual Plan? Plan { get; set; }

    public virtual User? PrimaryUser { get; set; }

    public virtual List<UserSubscription>? AdditionalUsers { get; set; }
}
