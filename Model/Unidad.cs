using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    public class Unidad
    {
        [Key]
        public int Id { get; set; }

        [Required,  MaxLength(100)]
        public string Nombre { get; set; }

        [Required, MaxLength(200)]
        public string NombrePropietario { get; set; }

        [Required, MaxLength(200)]
        public string ApellidoPropietario { get; set; }

        [Required, MaxLength(200)]
        [EmailAddress]
        public string EmailPropietario { get; set; }

        
        public int? Superficie { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        // Relación: La unidad pertenece a un Usuario y un Consorcio

        [Required]
        public int IdUsuarioCreador { get; set; }

        [ForeignKey("IdUsuarioCreador")]
        public virtual Usuario UsuarioCreador { get; set; }

        [Required]
        public int IdConsorcio { get; set; }

        [ForeignKey("IdConsorcio")]
        public virtual Consorcio Consorcio { get; set; }
    }
}
