using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;

namespace Services
{
    public class UnidadService : IUnidadService
    {
        private readonly ConsorcioContext _context;

        public UnidadService(ConsorcioContext context)
        {
            _context = context;
        }

        public List<Unidad> ObtenerUnidades(int idConsorcio, int usuarioId)
        {
            return _context.Unidades
                .Include(u => u.Consorcio)
                .Where(u => u.IdConsorcio == idConsorcio &&
                            u.IdUsuarioCreador == usuarioId)
                .OrderBy(u => u.Nombre)
                .ToList();
        }

        public Consorcio ObtenerConsorcio(int idConsorcio, int usuarioId)
        {
            return _context.Consorcios
                .FirstOrDefault(c => c.Id == idConsorcio &&
                                     c.IdUsuarioCreador == usuarioId);
        }

        public async Task AgregarUnidad(Unidad unidad, int usuarioId)
        {
            var consorcio = await _context.Consorcios
                .FirstOrDefaultAsync(c => c.Id == unidad.IdConsorcio &&
                                          c.IdUsuarioCreador == usuarioId);

            if (consorcio == null)
                throw new Exception("Consorcio no encontrado.");

            unidad.IdUsuarioCreador = usuarioId;
            unidad.FechaCreacion = DateTime.Now;

            _context.Unidades.Add(unidad);
            await _context.SaveChangesAsync();
        }

        public void EliminarUnidad(int id, int usuarioId)
        {
            var unidad = _context.Unidades
                .FirstOrDefault(u => u.Id == id &&
                                     u.IdUsuarioCreador == usuarioId);

            if (unidad != null)
            {
                _context.Unidades.Remove(unidad);
                _context.SaveChanges();
            }
        }
    }
}