using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.ViewModels
{
    public class GastoViewModel
    {
        public int Id { get; set; }

        // Id del consorcio al que pertenece el gasto. Viene de la URL, no es editable por el usuario.
        [Required]
        public int IdConsorcio { get; set; }

        // Solo para mostrar en la vista (no se bindea desde el form)
        public string? NombreConsorcio { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Nombre { get; set; }

        // No requerido según la consigna ("todos los campos son requeridos excepto Descripción")
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha del gasto es obligatoria")]
        public DateTime FechaGasto { get; set; }

        [Required(ErrorMessage = "El año de la expensa es obligatorio")]
        [Range(2000, 2100, ErrorMessage = "El año debe ser válido")]
        public int AnioExpensa { get; set; }

        [Required(ErrorMessage = "El mes de la expensa es obligatorio")]
        [Range(1, 12, ErrorMessage = "El mes debe estar entre 1 y 12")]
        public int MesExpensa { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El tipo de gasto es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El tipo de gasto es obligatorio")]
        public int IdTipoGasto { get; set; }

        // Nombre del archivo guardado en disco. En Crear se genera al subir el archivo.
        // En Editar se usa para mostrar el link de descarga.
        public string? ArchivoComprobante { get; set; }
    }
}
