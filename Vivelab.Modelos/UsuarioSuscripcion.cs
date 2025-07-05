using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class UsuarioSuscripcion
{
    [Key]
    public int Codigo { get; set; }

    public int SuscripcionCodigo { get; set; }

    public int UsuarioCodigo { get; set; }

    public virtual Suscripcion? Suscripcion { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
