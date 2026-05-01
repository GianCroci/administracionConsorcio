using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }

        public DateTime FechaRegistracion { get; set; }

        public DateTime? FechaUltLogin { get; set; }

      
        public virtual ICollection<Consorcio> Consorcios { get; set; }

       
        public virtual ICollection<Unidad> Unidades { get; set; }

        
        public virtual ICollection<Gasto> Gastos { get; set; }
    }
}