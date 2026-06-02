using Data;
using DTOs;
using DTOs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;

namespace Services.Tests
{
    public class UsuarioServiceTests
    {
        private readonly ConsorcioContext _context;
        private readonly IUsuarioService _service;

        public UsuarioServiceTests()
        {
            var options = new DbContextOptionsBuilder<ConsorcioContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ConsorcioContext(options);
            _service = new UsuarioService(_context);
        }


        [Fact]
        public async Task Registrar_EmailYaExiste_LanzaExcepcion()
        {
            // Arrange: metemos un usuario con ese email directamente en la base
            _context.Usuarios.Add(new Usuario
            {
                Email = "test@mail.com",
                Password = "hash",
                FechaRegistracion = DateTime.Now
            });
            await _context.SaveChangesAsync();

            var vm = new RegistroViewModel { Email = "test@mail.com", Password = "1234" };

            // Act & Assert: esperamos que explote con la excepción correcta
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.Registrar(vm));
            Assert.Equal("El correo electrónico ya se encuentra registrado.", ex.Message);
        }

        [Fact]
        public async Task RegistrarUsuario_Guarda_EnLaBase()
        {
            // Arrange: creamos un VM con datos válidos
            var vm = new RegistroViewModel { Email = "test@mail.com", Password = "1234" };

            // Act: registramos el usuario
            await _service.Registrar(vm);

            // Assert: verificamos que el usuario se guardó en la base
            var creado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == vm.Email);
            Assert.NotNull(creado);
        }

        [Fact]
        public async Task Login_NoEncuentraUsuario_DevuelveNull()
        {
            // Act: intentamos loguear con un email que no existe
            var result = await _service.Login("test@mail.com", "1234");

            // Assert: esperamos que el resultado sea null
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_ClaveIncorrecta_DevuelveNull()
        {
            //Arrange: creamos un usuario
            _context.Usuarios.Add(new Usuario
            {
                Email = "test@mail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("hash"),
                FechaRegistracion = DateTime.Now
            });
            await _context.SaveChangesAsync();

            //Act: intentamos loguear con la clave incorrecta
            var result = await _service.Login("test@mail.com", "incorrecta");

            //Assert: esperamos que el resultado sea null
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_ClaveCorrecta_DevuelveUsuario()
        {
            //Arrange: creamos un usuario
            _context.Usuarios.Add(new Usuario
            {
                Email = "test@mail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("hash"),
                FechaRegistracion = DateTime.Now
            });
            await _context.SaveChangesAsync();

            //Act: intentamos loguear con la clave correcta
            var result = await _service.Login("test@mail.com", "hash");

            //Assert: esperamos que devuelva un UsuarioLoginDto con los datos correctos
            var dto = Assert.IsType<UsuarioLoginDto>(result);
        }
    }
}
