using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Notificacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }

        [Required]
        public int IdConsorcio { get; set; }

        [ForeignKey("IdConsorcio")]
        public virtual Consorcio Consorcio { get; set; }

        [Required]
        public int IdUsuarioCreador { get; set; }

        [ForeignKey("IdUsuarioCreador")]
        public virtual Usuario UsuarioCreador { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Null mientras no se haya enviado. Al enviar se setea con DateTime.Now.
        public DateTime? FechaEnvio { get; set; }
    }
}