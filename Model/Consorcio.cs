using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Model
{
    public class Consorcio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(200)]
        public string Ciudad { get; set; }

        [Required]
        [MaxLength(200)]
        public string Calle { get; set; }

        [Required]
        public int Altura { get; set; }

        [Required]
        [Range(1, 28)]
        public int DiaVencimientoExpensas { get; set; }

        public DateTime FechaCreacion { get; set; }

        public double? Latitud { get; set; }
        public double? Longitud { get; set; }

        [Required]
        public int IdProvincia { get; set; }

        [ForeignKey("IdProvincia")]
        public virtual Provincia Provincia { get; set; }

        [Required]
        public int IdUsuarioCreador { get; set; }

        [ForeignKey("IdUsuarioCreador")]
        public virtual Usuario UsuarioCreador { get; set; }

        
        public virtual ICollection<Unidad> Unidades { get; set; }

        
        public virtual ICollection<Gasto> Gastos { get; set; }

        public virtual ICollection<Sum> Sums { get; set; }
    }
}
