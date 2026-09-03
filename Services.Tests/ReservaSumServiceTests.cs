using Data;
using DTOs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Tests
{
    public class ReservaSumServiceTests
    {
        private readonly ConsorcioContext _context;
        private readonly IReservaSumService _service;

        public ReservaSumServiceTests()
        {
            var options = new DbContextOptionsBuilder<ConsorcioContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ConsorcioContext(options);
            _service = new ReservaSumService(_context);
        }

        [Fact]
        public void ObtenerListaDeReservasPorIdDeSum()
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

            var sum = new Sum
            {
                Id = 1,
                IdConsorcio = 1,
                Nombre = "Sum"
            };
            _context.Sum.Add(sum);

            var reserva = new ReservaSum
            {
                Id = 1,
                IdSum = 1,
                FechaReserva = DateTime.Now,
                Turno = 0,
                Comentarios = "",
                UsuarioQueReserva = usuario
            };
            _context.ReservaSum.Add(reserva);
            var reserva2 = new ReservaSum
            {
                Id = 2,
                IdSum = 1,
                FechaReserva = DateTime.Now,
                Turno = (Turno)1,
                Comentarios = "",
                UsuarioQueReserva = usuario
            };
            _context.ReservaSum.Add(reserva2);
            _context.SaveChanges();

            //Act
            var result = _service.ObtenerReservas(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void ObtenerReservaPorNumeroDeId()
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

            var sum = new Sum
            {
                Id = 1,
                IdConsorcio = 1,
                Nombre = "Sum"
            };
            _context.Sum.Add(sum);

            var reserva = new ReservaSum
            {
                Id = 1,
                IdSum = 1,
                FechaReserva = DateTime.Now,
                Turno = 0,
                Comentarios = "",
                UsuarioQueReserva = usuario
            };
            _context.ReservaSum.Add(reserva);
            _context.SaveChanges();

            //Act
            var resultado = _service.ObtenerReservaPorId(1);

            //Assert
            Assert.NotNull(resultado);
            Assert.Equal(usuario, resultado.UsuarioQueReserva);
        }

        [Fact]
        public void QueSePuedaAgregarUnaReserva()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Email = "test@test.com",
                Password = "password"
            };
            _context.Usuarios.Add(usuario);

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);

            var sum = new Sum
            {
                Id = 1,
                IdConsorcio = 1,
                Nombre = "Sum"
            };
            _context.Sum.Add(sum);

            var reserva = new ReservaSumViewModel
            {
                Id = 1,
                IdSum = 1,
                FechaReserva = DateTime.Now,
                Turno = 0,
                Comentarios = "",
                IdUsuario = usuario.Id,
               
            };

            //Act
            _service.AgregarReserva(reserva, usuario.Id);

            var result = _service.ObtenerReservaPorId(reserva.Id);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void QueSePuedaEditarUnaReserva()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Email = "test@test.com",
                Password = "password"
            };
            _context.Usuarios.Add(usuario);

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);

            var sum = new Sum
            {
                Id = 1,
                IdConsorcio = 1,
                Nombre = "Sum"
            };
            _context.Sum.Add(sum);

            var reserva = new ReservaSum
            {
                Id = 1,
                IdSum = 1,
                FechaReserva = new DateTime(2026, 9, 10),
                Turno = 0,
                Comentarios = "",
                IdUsuario = usuario.Id,

            };
            _context.ReservaSum.Add(reserva);
            _context.SaveChanges();

            //Act
            var result = _service.ObtenerReservaPorId(1);

            var modificada = new ReservaSumViewModel
            {
                Id = reserva.Id,
                IdSum = reserva.IdSum,
                FechaReserva = reserva.FechaReserva,
                Turno = 0,
                Comentarios = "Esta reserva fue modificada",
                IdUsuario = reserva.IdUsuario,

            };

            _service.EditarReserva(modificada);

            var editada = _service.ObtenerReservaPorId(1);
            

            //Assert
            Assert.Equal("Esta reserva fue modificada", editada.Comentarios);
        }

        [Fact]
        public void QueSePuedaEliminarUnaReserva()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Email = "test@test.com",
                Password = "password"
            };
            _context.Usuarios.Add(usuario);

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);

            var sum = new Sum
            {
                Id = 1,
                IdConsorcio = 1,
                Nombre = "Sum"
            };
            _context.Sum.Add(sum);

            var reserva = new ReservaSum
            {
                Id = 1,
                IdSum = 1,
                FechaReserva = new DateTime(2026, 9, 10),
                Turno = 0,
                Comentarios = "",
                IdUsuario = usuario.Id,

            };
            _context.ReservaSum.Add(reserva);
            _context.SaveChanges();

            //Act
            _service.EliminarReserva(1);

            var result = _service.ObtenerReservaPorId(1);

            //Assert
            Assert.Null(result);
        }
    }
}
