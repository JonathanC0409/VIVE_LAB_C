using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class UserSubscription
{
    [Key]
    public int Code { get; set; }

    public int SubscriptionCode { get; set; }

    public int UserCode { get; set; }

    public virtual Subscription? Subscription { get; set; }

    public virtual User? User { get; set; }
}
