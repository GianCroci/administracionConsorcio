using Data;
using DTOs;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class ExpensaService : IExpensaService
    {
        private readonly ConsorcioContext _context;

        public ExpensaService(ConsorcioContext context)
        {
            _context = context;
        }

        public async Task<List<ExpensaDTO>> GetExpensasPorMes(int consorcioId, int usuarioId)
        {
            int cantidadUnidades = await _context.Unidades
                .Where(u => u.IdConsorcio == consorcioId )
                .CountAsync();

            if (cantidadUnidades == 0)
                return new List<ExpensaDTO>();

            var expensas = await _context.Gastos
                .Where(g => g.Consorcio.Id == consorcioId && g.Consorcio.IdUsuarioCreador == usuarioId
                && !(g.AnioExpensa == DateTime.Now.Year && g.MesExpensa == DateTime.Now.Month))
                .GroupBy(g => new { g.AnioExpensa, g.MesExpensa })
                .Select(g => new ExpensaDTO
                {
                    Año = g.Key.AnioExpensa.ToString(),
                    Mes = g.Key.MesExpensa.ToString(),
                    GastoMes = g.Sum(x => x.Monto),
                    MontoXUnidad = g.Sum(x => x.Monto) / cantidadUnidades
                })
                .ToListAsync();

            if (expensas == null)
                return null;

            expensas = expensas
                .OrderByDescending(e => int.Parse(e.Año))
                .ThenByDescending(e => int.Parse(e.Mes))
                .ToList();

            

            return expensas;
        }

        public async Task<ExpensaDTO> ObtenerDatosConsorcio(int consorcioId, int usuarioId)
        {
            var consorcio = await _context.Consorcios
                .FirstOrDefaultAsync(c => c.Id == consorcioId && c.IdUsuarioCreador == usuarioId);

            if (consorcio == null)
                return null;

            var ahora = DateTime.Now;

            var unidades = await _context.Unidades
                .Where(u => u.IdConsorcio == consorcioId)
                .CountAsync();

            decimal gastoMes = await _context.Gastos
                .Where(g => g.Consorcio.Id == consorcioId && g.Consorcio.IdUsuarioCreador == usuarioId
                && g.AnioExpensa == ahora.Year
                && g.MesExpensa == ahora.Month)
                .SumAsync(g => (decimal?)g.Monto) ?? 0;

            var expensa = new ExpensaDTO
            {
                ConsorcioId = consorcioId,
                ConsorcioNombre = consorcio.Nombre,
                Año = ahora.Year.ToString(),
                GastoMes = gastoMes,
                CantidadUnidades = unidades,
                MontoXUnidad = unidades > 0 ? gastoMes / unidades : 0
            };

            expensa.Mes = expensa.MesNombre(ahora.Month);
            return expensa;
        }
    }
}
