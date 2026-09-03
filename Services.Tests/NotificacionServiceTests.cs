using Data;
using DTOs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Model;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Tests
{
    public class NotificacionServiceTests
    {
        private readonly ConsorcioContext _context;
        private readonly INotificacionService _service;

        public NotificacionServiceTests()
        {
            var options = new DbContextOptionsBuilder<ConsorcioContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ConsorcioContext(options);
            _service = new NotificacionService(_context);
        }

        [Fact]
        public void ObtenerElLIstadoDeLasNotificaciones()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 10,
                Email = "test@test.com",
                Password = "password"
            };

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);

            var notificacion = new Notificacion
            {
                Id = 1,
                Titulo = "notificacion",
                Descripcion = "test",
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdUsuarioCreador = usuario.Id,
            };

            _context.Notificaciones.Add(notificacion);

            var notificacion2 = new Notificacion
            {
                Id = 2,
                Titulo = "notificacion",
                Descripcion = "test",
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdUsuarioCreador = usuario.Id,
            };

            _context.Notificaciones.Add(notificacion2);
            _context.SaveChanges();

            //Act
            var result = _service.ObtenerNotificaciones(consorcio.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void QueSePuedanObtenerNotificacionesPorSuId()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Email = "test@test.com",
                Password = "password"
            };

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);

            var notificacion = new Notificacion
            {
                Id = 1,
                Titulo = "notificacion",
                Descripcion = "test",
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdUsuarioCreador = usuario.Id,
            };

            _context.Notificaciones.Add(notificacion);
            _context.SaveChanges();

            //Act
            var result = _service.ObtenerNotificacionPorId(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.Descripcion);
        }

        [Fact]
        public void QueSePuedaAgregarUnaNotificacion()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Email = "test@test.com",
                Password = "password"
            };

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);

            var notificacion = new NotificacionViewModel
            {
                Id = 1,
                Titulo = "notificacion",
                Descripcion = "test",
                IdConsorcio = consorcio.Id
            };

            //Act
            _service.AgregarNotificacion(notificacion, usuario.Id);

            var result = _service.ObtenerNotificacionPorId(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("notificacion", result.Titulo);
            Assert.Equal("test", result.Descripcion);
        }

        [Fact]
        public void QueSePuedaEditarUnaNotificacion()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Email = "test@test.com",
                Password = "password"
            };

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);

            var notificacion = new NotificacionViewModel
            {
                Id = 1,
                Titulo = "notificacion",
                Descripcion = "test",
                IdConsorcio = consorcio.Id
            };

            _service.AgregarNotificacion(notificacion, usuario.Id);

            //Act
            notificacion.Titulo = "notificacion modificada";
            _service.EditarNotificacion(notificacion);
            var result = _service.ObtenerNotificacionPorId(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("notificacion modificada", result.Titulo);
        }

        [Fact]
        public void QueSePuedaEliminarUnaNotificacion()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Email = "test@test.com",
                Password = "password"
            };

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);

            var notificacion = new NotificacionViewModel
            {
                Id = 1,
                Titulo = "notificacion",
                Descripcion = "test",
                IdConsorcio = consorcio.Id
            };

            _service.AgregarNotificacion(notificacion, usuario.Id);

            //Act
            _service.EliminarNotificacion(notificacion.Id);
            var result = _service.ObtenerNotificacionPorId(1);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task QueSePuedaEnviarUnaNotificacion()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 10,
                Email = "test@test.com",
                Password = "password"
            };

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };
            _context.Consorcios.Add(consorcio);

            var unidad = new Unidad
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Nombre = "8 B",
                NombrePropietario = "test",
                ApellidoPropietario = "test",
                EmailPropietario = "test@test.com",
                IdConsorcio = 1
            };
            _context.Unidades.Add(unidad);
            

            var notificacion = new Notificacion
            {
                Id = 1,
                Titulo = "notificacion",
                Descripcion = "test",
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdUsuarioCreador = usuario.Id,
            };

            _context.Notificaciones.Add(notificacion);
            _context.SaveChanges();

            var emailServiceMock = new Mock<IEmailService>();

            //Act
            await _service.EnviarNotificacionAsync(1, emailServiceMock.Object);           

            //Assert

            var result = _service.ObtenerNotificacionPorId(1);

            Assert.NotNull(result.FechaEnvio);

            emailServiceMock.Verify(e =>
                e.EnviarAsync(
                    It.Is<List<string>>(emails =>
                        emails.Contains("test@test.com")),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);
        }
    }
}
