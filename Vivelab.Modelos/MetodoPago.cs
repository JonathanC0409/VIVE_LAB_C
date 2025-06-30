using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public  class MetodoPago
{
    [Key]
    public int Codigo { get; set; }

    public string PeiGoPaymentToken { get; set; } 

    public DateTime FechaRegistro { get; set; }

    public virtual List<Suscripcion>? Suscripciones { get; set; } 
}
