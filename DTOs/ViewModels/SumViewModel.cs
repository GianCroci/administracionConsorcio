using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ViewModels
{
    public class SumViewModel
    {
        public int Id { get; set; }

        [Required]
        public int IdConsorcio { get; set; }

        public string? NombreConsorcio { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Nombre { get; set; }

        public int CantidadReservas { get; set; }
    }
}
