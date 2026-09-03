using Data;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Tests
{
    public class ExpensaServiceTests
    {
        private readonly ConsorcioContext _context;
        private readonly IExpensaService _service;

        public ExpensaServiceTests()
        {
            var options = new DbContextOptionsBuilder<ConsorcioContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ConsorcioContext(options);
            _service = new ExpensaService(_context);
        }

        [Fact]
        public async Task QueSePuedaObtenerLasExpensasPorMes()
        {
            //Arrange
            var usuario = new Usuario
            {
                Email = "test@test.com",
                Password = "password"
            };
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            var consorcio = new Consorcio
            {
                IdUsuarioCreador = usuario.Id,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };
            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();

            var unidad = new Unidad
            {
                IdUsuarioCreador = usuario.Id,
                Nombre = "8 B",
                NombrePropietario = "test",
                ApellidoPropietario = "test",
                EmailPropietario = "test@test.com",
                IdConsorcio = consorcio.Id
            };

            _context.Unidades.Add(unidad);

            var unidad2 = new Unidad
            {
                IdUsuarioCreador = usuario.Id,
                Nombre = "8 A",
                NombrePropietario = "test",
                ApellidoPropietario = "test",
                EmailPropietario = "test@test.com",
                IdConsorcio = consorcio.Id
            };

            _context.Unidades.Add(unidad2);

            _context.SaveChanges();

            var tipoGasto = new TipoGasto
            {
                Nombre = "test"
            };
            _context.TiposGasto.Add(tipoGasto);
            _context.SaveChanges();

            var gasto = new Gasto
            {
                Nombre = "gasto",
                Descripcion = "",
                FechaGasto = new DateTime(2026, 8, 15),
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 200000,
                FechaCreacion = new DateTime(2026, 8, 15),
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdTipoGasto = tipoGasto.Id,
                TipoGasto = tipoGasto,
                IdUsuarioCreador = usuario.Id,
                UsuarioCreador = usuario
            };
            _context.Gastos.Add(gasto);

            var gasto2 = new Gasto
            {
                Nombre = "gasto",
                Descripcion = "",
                FechaGasto = new DateTime(2026, 8, 15),
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 100000,
                FechaCreacion = new DateTime(2026, 8, 15),
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdTipoGasto = tipoGasto.Id,
                TipoGasto = tipoGasto,
                IdUsuarioCreador = usuario.Id,
                UsuarioCreador = usuario
            };
            _context.Gastos.Add(gasto2);

            _context.SaveChanges(); 

            //Act
            List<ExpensaDTO> result = await _service.GetExpensasPorMes(consorcio.Id, usuario.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result); //Espera una unica fila de expensas
            Assert.Equal("2026", result[0].Año);
            Assert.Equal("8", result[0].Mes);
            Assert.Equal(300000, result[0].GastoMes);
            Assert.Equal(150000, result[0].MontoXUnidad);
        }

        [Fact]
        public async Task QueSePuedanObtenerLosDatosDelCOnsorcio()
        {
            //Arrange
            var usuario = new Usuario
            {
                Email = "test@test.com",
                Password = "password"
            };
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            var consorcio = new Consorcio
            {
                IdUsuarioCreador = usuario.Id,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test consorcio"
            };
            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();

            var unidad = new Unidad
            {
                IdUsuarioCreador = usuario.Id,
                Nombre = "8 B",
                NombrePropietario = "test",
                ApellidoPropietario = "test",
                EmailPropietario = "test@test.com",
                IdConsorcio = consorcio.Id
            };

            _context.Unidades.Add(unidad);

            var unidad2 = new Unidad
            {
                IdUsuarioCreador = usuario.Id,
                Nombre = "8 A",
                NombrePropietario = "test",
                ApellidoPropietario = "test",
                EmailPropietario = "test@test.com",
                IdConsorcio = consorcio.Id
            };

            _context.Unidades.Add(unidad2);

            _context.SaveChanges();

            var tipoGasto = new TipoGasto
            {
                Nombre = "test"
            };
            _context.TiposGasto.Add(tipoGasto);
            _context.SaveChanges();

            var gasto = new Gasto
            {
                Nombre = "gasto",
                Descripcion = "",
                FechaGasto = new DateTime(2026, 8, 15),
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 200000,
                FechaCreacion = new DateTime(2026, 8, 15),
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdTipoGasto = tipoGasto.Id,
                TipoGasto = tipoGasto,
                IdUsuarioCreador = usuario.Id,
                UsuarioCreador = usuario
            };
            _context.Gastos.Add(gasto);

            var gasto2 = new Gasto
            {
                Nombre = "gasto",
                Descripcion = "",
                FechaGasto = new DateTime(2026, 8, 15),
                AnioExpensa = 2026,
                MesExpensa = 8,
                ArchivoComprobante = "",
                Monto = 100000,
                FechaCreacion = new DateTime(2026, 8, 15),
                IdConsorcio = consorcio.Id,
                Consorcio = consorcio,
                IdTipoGasto = tipoGasto.Id,
                TipoGasto = tipoGasto,
                IdUsuarioCreador = usuario.Id,
                UsuarioCreador = usuario
            };
            _context.Gastos.Add(gasto2);

            _context.SaveChanges();

            //Act
            ExpensaDTO result = await _service.ObtenerDatosConsorcio(consorcio.Id, usuario.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(consorcio.Id, result.ConsorcioId);
            Assert.Equal(consorcio.Nombre, result.ConsorcioNombre);
        }
    }
}
