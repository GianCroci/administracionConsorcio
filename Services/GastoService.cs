using Data;
using DTOs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class GastoService : IGastoService
    {
        private readonly ConsorcioContext _context;

        public GastoService(ConsorcioContext context)
        {
            _context = context;
        }

        public List<Gasto> ObtenerGastos(int idConsorcio)
        {
            return _context.Gastos
                .Include(g => g.TipoGasto)
                .Include(g => g.Consorcio)
                .Where(g => g.IdConsorcio == idConsorcio)
                .OrderByDescending(g => g.FechaGasto)
                .ToList();
        }

        public Gasto ObtenerGastoPorId(int id)
        {
            return _context.Gastos
                .Include(g => g.TipoGasto)
                .Include(g => g.Consorcio)
                .FirstOrDefault(g => g.Id == id);
        }

        public void AgregarGasto(GastoViewModel gastoVm, int usuarioId, string archivoComprobante)
        {
            var gasto = new Gasto
            {
                Nombre = gastoVm.Nombre,
                Descripcion = gastoVm.Descripcion,
                FechaGasto = gastoVm.FechaGasto,
                AnioExpensa = gastoVm.AnioExpensa,
                MesExpensa = gastoVm.MesExpensa,
                Monto = gastoVm.Monto,
                IdConsorcio = gastoVm.IdConsorcio,
                IdTipoGasto = gastoVm.IdTipoGasto,
                IdUsuarioCreador = usuarioId,
                FechaCreacion = DateTime.Now,
                ArchivoComprobante = archivoComprobante
            };

            _context.Gastos.Add(gasto);
            _context.SaveChanges();
        }

        public void EditarGasto(GastoViewModel gastoVm, string nuevoArchivoComprobante)
        {
            var existente = _context.Gastos.FirstOrDefault(g => g.Id == gastoVm.Id);
            if (existente == null)
                throw new Exception("Gasto no encontrado");

            existente.Nombre = gastoVm.Nombre;
            existente.Descripcion = gastoVm.Descripcion;
            existente.FechaGasto = gastoVm.FechaGasto;
            existente.AnioExpensa = gastoVm.AnioExpensa;
            existente.MesExpensa = gastoVm.MesExpensa;
            existente.Monto = gastoVm.Monto;
            existente.IdTipoGasto = gastoVm.IdTipoGasto;

            // Solo reemplaza el archivo si se subió uno nuevo
            if (!string.IsNullOrEmpty(nuevoArchivoComprobante))
                existente.ArchivoComprobante = nuevoArchivoComprobante;

            _context.SaveChanges();
        }

        public void EliminarGasto(int id)
        {
            var gasto = _context.Gastos.Find(id);
            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
                _context.SaveChanges();
            }
        }

        public List<TipoGasto> ObtenerTiposGasto()
        {
            return _context.TiposGasto.ToList();
        }
    }
}
