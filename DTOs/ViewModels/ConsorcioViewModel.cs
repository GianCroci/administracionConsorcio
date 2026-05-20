using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.ViewModels
{
    public class ConsorcioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [MaxLength(200, ErrorMessage = "La ciudad no puede exceder los 200 caracteres")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "La calle es obligatoria")]
        [MaxLength(200,ErrorMessage ="La calle no puede exceder los 200 caracteres")]
        public string Calle { get; set; }

        [Required(ErrorMessage = "La altura es obligatoria")]
        [Range(1,int.MaxValue, ErrorMessage ="La altura debe ser un número positivo")]
        public int Altura { get; set; }

        [Required(ErrorMessage = "El día de vencimiento de expensas es obligatorio")]
        [Range(1,28,ErrorMessage ="El día de vencimiento de expensas debe estar entre 1 y 28")]
        public int DiaVencimientoExpensas { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria")]
        [Range(1,10, ErrorMessage ="La provincia es obligatoria")]
        public int IdProvincia { get; set; } 
    }
}
