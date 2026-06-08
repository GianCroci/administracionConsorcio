using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ViewModels
{
    public class NotificacionViewModel
    {
        public int Id { get; set; }

        [Required]
        public int IdConsorcio { get; set; }

        public string? NombreConsorcio { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [MaxLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Null si no fue enviada todavía
        public DateTime? FechaEnvio { get; set; }

        // Propiedad calculada para simplificar lógica en vistas y controller
        public bool FueEnviada => FechaEnvio.HasValue;
    }
}