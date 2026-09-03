using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Tests
{
    public class UnidadServiceTests
    {
        private readonly ConsorcioContext _context;
        private readonly IUnidadService _service;

        public UnidadServiceTests()
        {
            var options = new DbContextOptionsBuilder<ConsorcioContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ConsorcioContext(options);
            _service = new UnidadService(_context);
        }

        [Fact]
        public void ObtenerConsorcio_DebeDevolverElCOnsorcioDelUsuario()
        {
            //Arrange

            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();


            //Act

            var resultado = _service.ObtenerConsorcio(1, 10);

            //Result

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal(10, resultado.IdUsuarioCreador);
        }

        [Fact]
        public void ObtenerConsorcio_DeOtroUsuarioDebeDarNull()
        {
            //Arrange
            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();

            //Act

            var resultado = _service.ObtenerConsorcio(1, 5);

            //Assert

            Assert.Null(resultado);
        }

        [Fact]
        public void ObtenerConsorcio_UsandoUnIdIncorrectoDebeDarNull()
        {
            //Arrange
            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();

            //Act

            var resultado = _service.ObtenerConsorcio(2, 10);

            //Assert

            Assert.Null (resultado);
        }

        [Fact]
        public void ObtenerUnidad_DebeDevolverUnidadDelUsuario()
        {
            //Arrange

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
            _context.SaveChanges();

            //Act

            var resultado = _service.ObtenerUnidad(1, 10);

            //Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal(10, resultado.IdUsuarioCreador);
        }

        [Fact]
        public void ObtenerUnidades_DebeDevolverListaDeUnidadesDelConsorcio()
        {
            //Arrange
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

            var unidad2 = new Unidad
            {
                Id = 2,
                IdUsuarioCreador = 10,
                Nombre = "8 A",
                NombrePropietario = "test",
                ApellidoPropietario = "test",
                EmailPropietario = "test@test.com",
                IdConsorcio = 1
            };

            _context.Unidades.Add(unidad2);
            _context.SaveChanges();

            //Act

            var result = _service.ObtenerUnidades(1, 10);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AgregarUnidad_GuardaEnLaBase()
        {
            //Arrange
            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

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

            unidad.FechaCreacion = DateTime.Now;

            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();

            //Act
            await _service.AgregarUnidad(unidad, 10);

            //Assert
            var creado = await _context.Unidades.FirstOrDefaultAsync(u => u.EmailPropietario == unidad.EmailPropietario);

            Assert.NotNull(creado);

        }

        [Fact]
        public async Task EditarUnidad_DebeModificarEnLaBase()
        {
            //Arrange
            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

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

            unidad.FechaCreacion = DateTime.Now;

            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();

            //Act
            await _service.AgregarUnidad(unidad, 10);            
            unidad.NombrePropietario = "test modificado";
            await _service.EditarUnidad(unidad, 10);
            var modificado = await _context.Unidades.FirstOrDefaultAsync(u => u.Id == unidad.Id);

            //Assert
            Assert.NotNull(modificado);
            Assert.Equal("test modificado", modificado.NombrePropietario);

        }

        [Fact]
        public void EliminarUnidad_BorraDeLaBase()
        {
            //Arrange
            var consorcio = new Consorcio
            {
                Id = 1,
                IdUsuarioCreador = 10,
                Calle = "falsa",
                Ciudad = "prueba",
                Nombre = "test"
            };

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

            unidad.FechaCreacion = DateTime.Now;

            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();

            //Act
            _service.AgregarUnidad(unidad, 10);

            var buscado = _context.Unidades.FirstOrDefault(u => u.Id == 1);
            _service.EliminarUnidad(buscado.Id, buscado.IdUsuarioCreador);

            var borrado = _context.Unidades.FirstOrDefault(u =>u.Id == 1);

            //Assert
            Assert.Null(borrado);

        }
    }
}
