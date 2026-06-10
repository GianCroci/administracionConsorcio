using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Moq;
using Moq.Protected;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Services.Tests
{
    public class ConsorcioServiceTest
    {

        private readonly ConsorcioContext _context;
        private readonly IConsorcioService _service;


        public ConsorcioServiceTest()
        {
            var options = new DbContextOptionsBuilder<ConsorcioContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ConsorcioContext(options);

            var geocodingMock = new Mock<IGeocodingService>();

            geocodingMock
                .Setup(x => x.GetCoordinates(It.IsAny<string>()))
                .ReturnsAsync((-34.6037, -58.3816));

            _service = new ConsorcioService(_context, geocodingMock.Object);
        }

        [Fact]
        public async Task AgregarConsorcio_GuardaEnLaBase()
        {
            _context.Provincias.Add(new Model.Provincia { Id = 1, Nombre = "Buenos Aires" });
            _context.SaveChanges();


            var vm = new DTOs.ViewModels.ConsorcioViewModel
            {
                Nombre = "Consorcio Test",
                Ciudad = "Ciudad Test",
                Calle = "Calle test",
                Altura = 100,
                DiaVencimientoExpensas = 5,
                IdProvincia = 1
            };

            int usuarioId = 1;
            int nuevoId = await _service.AgregarConsorcio(vm, usuarioId);


            var creado = await _context.Consorcios.FindAsync(nuevoId);
            Assert.NotNull(creado);
            Assert.Equal(vm.Nombre, creado.Nombre);

        }

        [Fact]
        public async Task AgregarConsorcioConNombreRepetidoLanzaException()
        {
            _context.Provincias.Add(new Model.Provincia { Id = 1, Nombre = "Buenos Aires" });
            _context.SaveChanges();

            _context.Consorcios.Add(new Consorcio
            {
                Nombre = "Consorcio Test",
                Calle = "Calle 1",
                Ciudad = "Ciudad 1",
            });
            _context.SaveChanges();

            var vm = new DTOs.ViewModels.ConsorcioViewModel
            {
                Nombre = "Consorcio Test",
                Ciudad = "Ciudad Test",
                Calle = "Calle test",
                Altura = 100,
                DiaVencimientoExpensas = 5,
                IdProvincia = 1
            };

            int usuarioId = 1;
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.AgregarConsorcio(vm, usuarioId));
            Assert.Equal("El consorcio ya se encuentra registrado.", ex.Message);

        }

        [Fact]
        public async Task EditarConsorcioConNombreRepetidoLanzaException()
        {
            _context.Provincias.Add(new Model.Provincia { Id = 1, Nombre = "Buenos Aires" });
            _context.SaveChanges();
            _context.Consorcios.AddRange(
                new Consorcio
                {
                    Id = 1,
                    Nombre = "Consorcio Test",
                    Calle = "Calle 1",
                    Ciudad = "Ciudad 1",
                    IdProvincia = 1
                },
                new Consorcio
                {
                    Id = 2,
                    Nombre = "Consorcio Test 2",
                    Calle = "Calle 2",
                    Ciudad = "Ciudad 2",
                    IdProvincia = 1
                }
            );
            _context.SaveChanges();
            var vm = new DTOs.ViewModels.ConsorcioViewModel
            {
                Id = 2,
                Nombre = "Consorcio Test",
                Ciudad = "Ciudad Test",
                Calle = "Calle test",
                Altura = 100,
                DiaVencimientoExpensas = 5,
                IdProvincia = 1
            };
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.EditarConsorcio(vm));
            Assert.Equal("El consorcio ya se encuentra registrado", ex.Message);
        }

        [Fact]
        public async Task EditarConsorcioConIdInexistenteLanzaException()
        {
            _context.Provincias.Add(new Model.Provincia { Id = 1, Nombre = "Buenos Aires" });
            _context.SaveChanges();
            _context.Consorcios.AddRange(
                new Consorcio
                {
                    Id = 1,
                    Nombre = "Consorcio Test",
                    Calle = "Calle 1",
                    Ciudad = "Ciudad 1",
                    IdProvincia = 1
                },
                new Consorcio
                {
                    Id = 2,
                    Nombre = "Consorcio Test 2",
                    Calle = "Calle 2",
                    Ciudad = "Ciudad 2",
                    IdProvincia = 1
                }
            );
            _context.SaveChanges();
            var vm = new DTOs.ViewModels.ConsorcioViewModel
            {
                Id = 999,
                Nombre = "Consorcio Test",
                Ciudad = "Ciudad Test",
                Calle = "Calle test",
                Altura = 100,
                DiaVencimientoExpensas = 5,
                IdProvincia = 1
            };
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.EditarConsorcio(vm));
            Assert.Equal("Consorcio no encontrado", ex.Message);
        }

        [Fact]
        public async Task QueSePuedaEditarLaInformacionDeUnConsorcio()
        {
            _context.Provincias.Add(new Model.Provincia { Id = 1, Nombre = "Buenos Aires" });
            _context.SaveChanges();
            _context.Consorcios.AddRange(
                new Consorcio
                {
                    Id = 1,
                    Nombre = "Consorcio Test",
                    Calle = "Calle 1",
                    Ciudad = "Ciudad 1",
                    IdProvincia = 1
                },
                new Consorcio
                {
                    Id = 2,
                    Nombre = "Consorcio Test 2",
                    Calle = "Calle 2",
                    Ciudad = "Ciudad 2",
                    IdProvincia = 1
                }
            );
            _context.SaveChanges();
            var vm = new DTOs.ViewModels.ConsorcioViewModel
            {
                Id = 2,
                Nombre = "Consorcio Editado",
                Ciudad = "Ciudad Editado",
                Calle = "Calle Editado",
                Altura = 100,
                DiaVencimientoExpensas = 5,
                IdProvincia = 1
            };

            await _service.EditarConsorcio(vm);
            var editado = await _context.Consorcios.FindAsync(2);
            Assert.Equal(editado.Nombre, vm.Nombre);
            Assert.Equal(editado.Calle, vm.Calle);
            Assert.Equal(editado.Provincia.Id, vm.IdProvincia);
        }

        [Fact]
        public async Task QueSePuedaEliminarUnConsorcio()
        {
            _context.Provincias.Add(new Model.Provincia { Id = 1, Nombre = "Buenos Aires" });
            _context.SaveChanges();
            _context.Consorcios.AddRange(
                new Consorcio
                {
                    Id = 1,
                    Nombre = "Consorcio Test",
                    Calle = "Calle 1",
                    Ciudad = "Ciudad 1",
                    IdProvincia = 1
                },
                new Consorcio
                {
                    Id = 2,
                    Nombre = "Consorcio Test 2",
                    Calle = "Calle 2",
                    Ciudad = "Ciudad 2",
                    IdProvincia = 1
                }
            );
            _context.SaveChanges();
            await _service.EliminarConsorcio(1);

            var cantConsorcios = await _context.Consorcios.CountAsync();
            Assert.Equal(1, cantConsorcios);
        }

        [Fact]
        public async Task QueMandeExceptionAlEliminarConsorcioInexistente()
        {
            _context.Provincias.Add(new Model.Provincia { Id = 1, Nombre = "Buenos Aires" });
            _context.SaveChanges();
            _context.Consorcios.AddRange(
                new Consorcio
                {
                    Id = 1,
                    IdUsuarioCreador = 1,
                    Nombre = "Consorcio Test",
                    Calle = "Calle 1",
                    Ciudad = "Ciudad 1",
                    IdProvincia = 1
                }
            );
            _context.SaveChanges();

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.EliminarConsorcio(999));
            Assert.Equal("Consorcio no encontrado", ex.Message);
        }


        [Fact]
        public async Task QueSePuedaObtenerTodosLosConsorciosDeUnUsuario()
        {
            _context.Provincias.Add(new Model.Provincia { Id = 1, Nombre = "Buenos Aires" });
            _context.SaveChanges();
            _context.Consorcios.AddRange(
                new Consorcio
                {
                    Id = 1,
                    IdUsuarioCreador = 1,
                    Nombre = "Consorcio Test",
                    Calle = "Calle 1",
                    Ciudad = "Ciudad 1",
                    IdProvincia = 1
                },
                new Consorcio
                {
                    Id = 2,
                    IdUsuarioCreador = 1,
                    Nombre = "Consorcio Test 2",
                    Calle = "Calle 2",
                    Ciudad = "Ciudad 2",
                    IdProvincia = 1
                },
                new Consorcio
                {
                    Id = 3,
                    IdUsuarioCreador = 2,
                    Nombre = "Consorcio Test 3",
                    Calle = "Calle 3",
                    Ciudad = "Ciudad 3",
                    IdProvincia = 1
                }
            );
            _context.SaveChanges();

            List<Consorcio> consorcios = _service.ObtenerConsorcios(1);
            Assert.Equal(2, consorcios.Count);
        }
    }
}
