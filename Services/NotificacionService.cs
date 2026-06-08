using Data;
using DTOs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly ConsorcioContext _context;

        public NotificacionService(ConsorcioContext context)
        {
            _context = context;
        }

        public List<Notificacion> ObtenerNotificaciones(int idConsorcio)
        {
            return _context.Notificaciones
                .Include(n => n.Consorcio)
                .Where(n => n.IdConsorcio == idConsorcio)
                .OrderByDescending(n => n.FechaCreacion)
                .ToList();
        }

        public Notificacion ObtenerNotificacionPorId(int id)
        {
            return _context.Notificaciones
                .Include(n => n.Consorcio)
                    .ThenInclude(c => c.Unidades)
                .FirstOrDefault(n => n.Id == id);
        }

        public void AgregarNotificacion(NotificacionViewModel vm, int usuarioId)
        {
            var notificacion = new Notificacion
            {
                Titulo = vm.Titulo,
                Descripcion = vm.Descripcion,
                IdConsorcio = vm.IdConsorcio,
                IdUsuarioCreador = usuarioId,
                FechaCreacion = DateTime.Now,
                FechaEnvio = null
            };

            _context.Notificaciones.Add(notificacion);
            _context.SaveChanges();
        }

        public void EditarNotificacion(NotificacionViewModel vm)
        {
            var existente = _context.Notificaciones.FirstOrDefault(n => n.Id == vm.Id);
            if (existente == null)
                throw new Exception("Notificación no encontrada");

            if (existente.FechaEnvio.HasValue)
                throw new Exception("No se puede editar una notificación ya enviada");

            existente.Titulo = vm.Titulo;
            existente.Descripcion = vm.Descripcion;
            _context.SaveChanges();
        }

        public void EliminarNotificacion(int id)
        {
            var notificacion = _context.Notificaciones.Find(id);
            if (notificacion == null) return;

            if (notificacion.FechaEnvio.HasValue)
                throw new Exception("No se puede eliminar una notificación ya enviada");

            _context.Notificaciones.Remove(notificacion);
            _context.SaveChanges();
        }

        public async Task EnviarNotificacionAsync(int id, EmailService emailService)
        {
            var notificacion = _context.Notificaciones
                .Include(n => n.Consorcio)
                    .ThenInclude(c => c.Unidades)
                .FirstOrDefault(n => n.Id == id);

            if (notificacion == null)
                throw new Exception("Notificación no encontrada");

            if (notificacion.FechaEnvio.HasValue)
                throw new Exception("La notificación ya fue enviada");

            var emails = notificacion.Consorcio.Unidades
                .Select(u => u.EmailPropietario)
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct()
                .ToList();

            if (emails.Any())
            {
                var asunto = $"[{notificacion.Consorcio.Nombre}] {notificacion.Titulo}";
                var cuerpo = $@"
                    <h2>{notificacion.Titulo}</h2>
                    <p>{notificacion.Descripcion}</p>
                    <hr/>
                    <small>Consorcio: {notificacion.Consorcio.Nombre}</small>
                ";
                await emailService.EnviarAsync(emails, asunto, cuerpo);
            }

            notificacion.FechaEnvio = DateTime.Now;
            _context.SaveChanges();
        }
    }
}