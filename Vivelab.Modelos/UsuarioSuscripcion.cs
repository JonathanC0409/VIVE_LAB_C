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

    public Suscripcion? Suscripcion { get; set; }

    public Usuario? Usuario { get; set; }
}
