using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class Suscripcion
{
    [Key]
    public int Codigo { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Estado { get; set; }

    public int PlanCodigo { get; set; }

    public int UsuarioCodigo { get; set; }

    public Plan? Plan { get; set; }

    public Usuario? UsuarioPrincipal { get; set; }

    public List<UsuarioSuscripcion>? UsuariosAdicionales { get; set; }
}
