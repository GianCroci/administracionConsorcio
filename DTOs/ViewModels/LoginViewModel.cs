using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Credenciales incorrectas")]
        [MaxLength(200)]
        public string Password { get; set; }
    }
}
