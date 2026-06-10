using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ViewModels
{
    public class ReservaSumViewModel
    {
        public int Id { get; set; }

        [Required]
        public int IdSum { get; set; }

        public string? NombreSum { get; set; }

        [Required(ErrorMessage = "La fecha de reserva es obligatoria")]
        public DateTime FechaReserva { get; set; }

        [Required(ErrorMessage = "El turno es obligatorio")]
        public int Turno { get; set; }  // 0 = Día, 1 = Noche

        [MaxLength(500, ErrorMessage = "Los comentarios no pueden exceder los 500 caracteres")]
        public string? Comentarios { get; set; }

        public bool EntregoCorrectamente { get; set; }

        public int IdUsuario { get; set; }

        public string? NombreUsuario { get; set; }
    }
}
