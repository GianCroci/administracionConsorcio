using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class ReservaSum
    {
        [Key]
        public int Id { get; set; }

        public int IdSum { get; set; }
        [ForeignKey("IdSum")]
        public virtual Sum Sum { get; set; }

        [Required]
        public DateTime FechaReserva { get; set; }

        [Required]
        public Turno Turno { get; set; }

        [MaxLength(500)]
        public string Comentarios  { get; set; }

        public bool EntregoCorrectamente { get; set; }

        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuario UsuarioQueReserva { get; set; }
        
        public virtual ICollection<Sum> Reservas { get; set; }
    }
}
public enum Turno
{
    Dia,
    Noche
}
