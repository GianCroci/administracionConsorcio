using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    public class Gasto
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [MaxLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaGasto { get; set; }
        [Required]
        public int AnioExpensa { get; set; }

        [Required]
        [Range(1, 12)]
        public int MesExpensa { get; set; }

        [Required]
        [MaxLength(500)]
        public string ArchivoComprobante { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public int IdConsorcio { get; set; }

        [ForeignKey("IdConsorcio")]
        public virtual Consorcio Consorcio { get; set; }

        [Required]
        public int IdTipoGasto { get; set; }

        [ForeignKey("IdTipoGasto")]
        public virtual TipoGasto TipoGasto { get; set; }

        [Required]
        public int IdUsuarioCreador { get; set; }

        [ForeignKey("IdUsuarioCreador")]
        public virtual Usuario UsuarioCreador { get; set; }
    }
}
