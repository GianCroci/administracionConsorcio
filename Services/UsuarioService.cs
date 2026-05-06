using Data;
using Model;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using DTOs.ViewModels;
using DTOs;

namespace Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly ConsorcioContext _context;

        public UsuarioService(ConsorcioContext context)
        {
            _context = context;
        }

        public async Task Registrar(RegistroViewModel vm)
        {
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == vm.Email);
            if (existe)
            {
                throw new Exception("El correo electrónico ya se encuentra registrado.");
            }


            var usuario = new Usuario
            {
                Email = vm.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(vm.Password),
                FechaRegistracion = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<UsuarioLoginDto> Login(string email, string password)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                return null;
            }
            
            bool esValida = BCrypt.Net.BCrypt.Verify(password, usuario.Password);
            
            if (esValida)
            {
                usuario.FechaUltLogin = DateTime.Now;
                await _context.SaveChangesAsync();

                return new UsuarioLoginDto
                {
                    Id = usuario.Id,
                    Email = usuario.Email
                };
            }

            return null;

        }

    }
}
