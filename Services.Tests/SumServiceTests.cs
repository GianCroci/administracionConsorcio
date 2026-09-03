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
    public class SumServiceTests
    {
        private readonly ConsorcioContext _context;
        private readonly ISumService _service;

        public SumServiceTests()
        {
            var options = new DbContextOptionsBuilder<ConsorcioContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ConsorcioContext(options);
            _service = new SumService(_context);
        }

        [Fact]
        public void ObtenerSumPorId()
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

            var sum = new Sum
            {
                Id = 1,
                IdConsorcio = 1,
                Nombre = "Sum"
            };
            _context.Sum.Add(sum);
            _context.SaveChanges();

            //Act

            var resultado = _service.ObtenerSumPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(sum.Id, resultado.Id);
        }

        [Fact]
        public void ObtenerListadoDeSums()
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

            var sum = new Sum
            {
                Id = 1,
                IdConsorcio = 1,
                Nombre = "Sum"
            };
            _context.Sum.Add(sum);

            var sum2 = new Sum
            {
                Id = 2,
                IdConsorcio = 1,
                Nombre = "Sum2"
            };
            _context.Sum.Add(sum2);


            _context.SaveChanges();

            //Act

            var resultado = _service.ObtenerSums(1);

            //Assert

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
        }

        [Fact]
        public void AgregarSum_DebeGuardarEnElContexto()
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

            var sum = new SumViewModel
            {
                Id = 1,
                IdConsorcio = 1,
                Nombre = "Sum"
            };
            _service.AgregarSum(sum);

            //Act
            var resultado = _context.Sum.FirstOrDefault(s => s.Id == sum.Id);

            //Assert
            Assert.NotNull(resultado);
            Assert.Equal("Sum", resultado.Nombre);
        }

        [Fact]
        public void QueSePuedanEditarDatosDelSum()
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

            var sum = new SumViewModel
            {
                Id = 1,
                IdConsorcio = 1,
                Nombre = "Sum"
            };
            _service.AgregarSum(sum);

            //Act
            sum.Nombre = "sum modificado";
            _service.EditarSum(sum);

            var buscado = _service.ObtenerSumPorId(1);

            //Assert
            Assert.NotNull(buscado);
            Assert.Equal("sum modificado", buscado.Nombre);
        }
    }
}
