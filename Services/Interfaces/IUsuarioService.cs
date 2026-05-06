using DTOs;
using DTOs.ViewModels;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IUsuarioService
    {
        Task Registrar(RegistroViewModel vm);
        Task<UsuarioLoginDto> Login(string email, string password);
    }
}
