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

        public int MetodoPagoCodigo { get; set; }

        public int UsuarioCodigo { get; set; }

        public virtual MetodoPago? MetodoPago { get; set; } 

        public virtual Plan? Plan { get; set; } 

        public virtual Usuario? UsuarioPrincipal { get; set; }

        public virtual List<UsuarioSuscripcion>? UsuariosAdicionales { get; set; }
    }
