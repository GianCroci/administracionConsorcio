using Data;
using DTOs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ReservaSumService : IReservaSumService
    {
        private readonly ConsorcioContext _context;

        public ReservaSumService(ConsorcioContext context)
        {
            _context = context;
        }

        public List<ReservaSum> ObtenerReservas(int idSum)
        {
            return _context.ReservaSum
                .Include(r => r.Sum)
                .Include(r => r.UsuarioQueReserva)
                .Where(r => r.IdSum == idSum)
                .OrderByDescending(r => r.FechaReserva)
                .ToList();
        }

        public ReservaSum ObtenerReservaPorId(int id)
        {
            return _context.ReservaSum
                .Include(r => r.Sum)
                .Include(r => r.UsuarioQueReserva)
                .FirstOrDefault(r => r.Id == id);
        }

        public void AgregarReserva(ReservaSumViewModel reservaVm, int usuarioId)
        {
            var reserva = new ReservaSum
            {
                IdSum = reservaVm.IdSum,
                FechaReserva = reservaVm.FechaReserva,
                Turno = (Model.Turno)reservaVm.Turno,
                Comentarios = reservaVm.Comentarios,
                EntregoCorrectamente = reservaVm.EntregoCorrectamente,
                IdUsuario = usuarioId
            };

            _context.ReservaSum.Add(reserva);
            _context.SaveChanges();
        }

        public void EditarReserva(ReservaSumViewModel reservaVm)
        {
            var existente = _context.ReservaSum.FirstOrDefault(r => r.Id == reservaVm.Id);
            if (existente == null)
                throw new Exception("Reserva no encontrada");

            existente.FechaReserva = reservaVm.FechaReserva;
            existente.Turno = (Model.Turno)reservaVm.Turno;
            existente.Comentarios = reservaVm.Comentarios;
            existente.EntregoCorrectamente = reservaVm.EntregoCorrectamente;

            _context.SaveChanges();
        }

        public void EliminarReserva(int id)
        {
            var reserva = _context.ReservaSum.Find(id);
            if (reserva != null)
            {
                _context.ReservaSum.Remove(reserva);
                _context.SaveChanges();
            }
        }
    }
}
