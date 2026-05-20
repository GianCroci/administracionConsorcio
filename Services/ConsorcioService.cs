using Data;
using DTOs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class ConsorcioService : IConsorcioService
    {
        private readonly ConsorcioContext _context;
        private readonly GeocodingService _geocodingService;

        public ConsorcioService(ConsorcioContext context, GeocodingService geocodingService)
        {
            _context = context;
            _geocodingService = geocodingService;
        }

        public async Task AgregarConsorcio(ConsorcioViewModel consorcioVm, int usuarioId)
        {

            var consorcio = new Consorcio
            {
                IdUsuarioCreador = usuarioId,
                Nombre = consorcioVm.Nombre,
                Ciudad = consorcioVm.Ciudad,
                Calle = consorcioVm.Calle,
                IdProvincia = consorcioVm.IdProvincia,
                Altura = consorcioVm.Altura,
                DiaVencimientoExpensas = consorcioVm.DiaVencimientoExpensas
            };

            var provincia = await _context.Provincias
            .FirstOrDefaultAsync(p => p.Id == consorcioVm.IdProvincia);

            var direccion = $"{consorcioVm.Calle} {consorcioVm.Altura}, {consorcioVm.Ciudad}, {provincia.Nombre}, Argentina";
            var coordenadas = await _geocodingService.GetCoordinates(direccion);

            consorcio.Latitud = coordenadas.lat;
            consorcio.Longitud = coordenadas.lon;

            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();
        }

        public async Task EditarConsorcio(ConsorcioViewModel consorcioVm)
        {

            var existente = _context.Consorcios
                .Include(c => c.Provincia)
                .FirstOrDefault(c => c.Id == consorcioVm.Id);

            if (existente == null)
                throw new Exception("Consorcio no encontrado");


            existente.Nombre = consorcioVm.Nombre;
            existente.Ciudad = consorcioVm.Ciudad;
            existente.Calle = consorcioVm.Calle;
            existente.IdProvincia = consorcioVm.IdProvincia;
            existente.Altura = consorcioVm.Altura;
            existente.DiaVencimientoExpensas = consorcioVm.DiaVencimientoExpensas;

            var provincia = await _context.Provincias
            .FirstOrDefaultAsync(p => p.Id == consorcioVm.IdProvincia);

            var direccion = $"{consorcioVm.Calle} {consorcioVm.Altura}, {consorcioVm.Ciudad}, {provincia.Nombre}, Argentina";
            var coordenadas = await _geocodingService.GetCoordinates(direccion);

            existente.Latitud = coordenadas.lat;
            existente.Longitud = coordenadas.lon;

            _context.SaveChanges();
        }

        public void EliminarConsorcio(int id)
        {
            var consorcio = _context.Consorcios.Find(id);

            if (consorcio != null)
            {
                _context.Consorcios.Remove(consorcio);
                _context.SaveChanges();
            }
        }

        public List<Consorcio> ObtenerConsorcios(int usuarioId)
        {
            return _context.Consorcios
        .Include(c => c.Provincia)
        .Where(c => c.IdUsuarioCreador == usuarioId)
        .ToList();
        }

        public List<Provincia> obtenerProvincias()
        {
            return _context.Provincias.ToList();
        }
    }
}
