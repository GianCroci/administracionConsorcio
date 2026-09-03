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
    public class GastoServiceTests
    {
        private readonly ConsorcioContext _context;
        private readonly IGastoService _service;

        public GastoServiceTests()
        {
            var options = new DbContextOptionsBuilder<ConsorcioContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ConsorcioContext(options);
            _service = new GastoService(_context);
        }

        [Fact]
        public void QueSePuedaObtenerLaListaDeGastos()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 10,
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

            var tipoGasto = new TipoGasto
            {
                Id = 1,
                Nombre = "test"
            };
            _context.TiposGasto.Add(tipoGasto);

            var gasto = new Gasto
            {
                Id = 1,
                Nombre = "gasto",
                Descripcion = "",
                FechaGasto = DateTime.Now,
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 200000,
                FechaCreacion = DateTime.Now,
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdTipoGasto = 1,
                TipoGasto = tipoGasto,
                IdUsuarioCreador = usuario.Id,
                UsuarioCreador = usuario
            };
            _context.Gastos.Add(gasto);

            var gasto2 = new Gasto
            {
                Id = 2,
                Nombre = "gasto2",
                Descripcion = "",
                FechaGasto = DateTime.Now,
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 200000,
                FechaCreacion = DateTime.Now,
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdTipoGasto = 1,
                TipoGasto = tipoGasto,
                IdUsuarioCreador = usuario.Id,
                UsuarioCreador = usuario
            };
            _context.Gastos.Add(gasto2);

            _context.SaveChanges();

            //Act
            var result = _service.ObtenerGastos(consorcio.Id);

            //Arrange
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void QueSePuedaObtenerUnGastoPorSuId()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 10,
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

            var tipoGasto = new TipoGasto
            {
                Id = 1,
                Nombre = "test"
            };
            _context.TiposGasto.Add(tipoGasto);

            var gasto = new Gasto
            {
                Id = 1,
                Nombre = "gasto",
                Descripcion = "",
                FechaGasto = DateTime.Now,
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 200000,
                FechaCreacion = DateTime.Now,
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdTipoGasto = 1,
                TipoGasto = tipoGasto,
                IdUsuarioCreador = usuario.Id,
                UsuarioCreador = usuario
            };
            _context.Gastos.Add(gasto);
            _context.SaveChanges();

            //Act
            var result = _service.ObtenerGastoPorId(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("gasto", result.Nombre);

        }

        [Fact]
        public void QueSePuedaAgregarUnGasto()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 10,
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

            var tipoGasto = new TipoGasto
            {
                Id = 1,
                Nombre = "test"
            };
            _context.TiposGasto.Add(tipoGasto);
            _context.SaveChanges();

            var gasto = new GastoViewModel
            {
                Id = 1,
                IdConsorcio = consorcio.Id,
                Nombre = "gasto",
                Descripcion = "",
                FechaGasto = DateTime.Now,
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 200000,
                IdTipoGasto = 1
            };

            //Act
            _service.AgregarGasto(gasto, usuario.Id, "archivo.pdf");

            var creado = _context.Gastos.First();

            //Assert
            var result = _service.ObtenerGastoPorId(creado.Id);
            Assert.NotNull(result);
            Assert.Equal("gasto", result.Nombre);
        }

        [Fact]
        public void QueSePuedaEditarUnGasto()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 10,
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

            var tipoGasto = new TipoGasto
            {
                Id = 1,
                Nombre = "test"
            };
            _context.TiposGasto.Add(tipoGasto);
            _context.SaveChanges();

            var gasto = new GastoViewModel
            {
                IdConsorcio = consorcio.Id,
                Nombre = "gasto",
                Descripcion = "",
                FechaGasto = DateTime.Now,
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 200000,
                IdTipoGasto = 1
            };

            //Act
            _service.AgregarGasto(gasto, usuario.Id, "archivo.pdf");
            var creado = _context.Gastos.First();

            gasto.Id = creado.Id;
            gasto.Nombre = "gasto Modificado";
            _service.EditarGasto(gasto, "archivo.pdf");

            //Assert
            var result = _service.ObtenerGastoPorId(gasto.Id);
            Assert.NotNull(result);
            Assert.Equal("gasto Modificado", result.Nombre);
        }

        [Fact]
        public void QueSePuedaEliminarUnGasto()
        {
            //Arrange
            var usuario = new Usuario
            {
                Id = 10,
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

            var tipoGasto = new TipoGasto
            {
                Id = 1,
                Nombre = "test"
            };
            _context.TiposGasto.Add(tipoGasto);
            _context.SaveChanges();

            var gasto = new GastoViewModel
            {
                IdConsorcio = consorcio.Id,
                Nombre = "gasto",
                Descripcion = "",
                FechaGasto = DateTime.Now,
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 200000,
                IdTipoGasto = 1
            };

            _service.AgregarGasto(gasto, usuario.Id, "archivo.pdf");

            //Act
            var creado = _context.Gastos.First();
            _service.EliminarGasto(creado.Id);

            //Assert
            var result = _service.ObtenerGastoPorId(creado.Id);
            Assert.Null(result);
        }

        [Fact]
        public void ObtenerElListadoDeTiposDeGastos()
        {
            //Arrange
            var tipoGasto = new TipoGasto
            {
                Nombre = "test"
            };
            _context.TiposGasto.Add(tipoGasto);
            var tipoGasto2 = new TipoGasto
            {
                Nombre = "test2"
            };
            _context.TiposGasto.Add(tipoGasto2);
            _context.SaveChanges();

            //Act
            var result = _service.ObtenerTiposGasto();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
