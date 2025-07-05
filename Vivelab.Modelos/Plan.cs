using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vivelab.Modelos;

public class Plan
{
    [Key]
    public int Codigo { get; set; }

    public string Nombre { get; set; }

    public double Precio { get; set; }

    public int CantidadUsuarios { get; set; }

    public string Descripcion { get; set; }

    public virtual List<Suscripcion>? Suscripciones { get; set; }
}
