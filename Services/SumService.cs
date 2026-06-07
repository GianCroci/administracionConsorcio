using Data;
using DTOs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;

namespace Services
{
    public class SumService : ISumService
    {
        private readonly ConsorcioContext _context;

        public SumService(ConsorcioContext context)
        {
            _context = context;
        }

        public List<Sum> ObtenerSums(int idConsorcio)
        {
            return _context.Sum
                .Include(s => s.Consorcio)
                .Include(s => s.Reservas)
                .Where(s => s.IdConsorcio == idConsorcio)
                .OrderBy(s => s.Nombre)
                .ToList();
        }

        public Sum ObtenerSumPorId(int id)
        {
            return _context.Sum
                .Include(s => s.Consorcio)
                .Include(s => s.Reservas)
                .FirstOrDefault(s => s.Id == id);
        }

        public void AgregarSum(SumViewModel vm)
        {
            var sum = new Sum
            {
                IdConsorcio = vm.IdConsorcio,
                Nombre = vm.Nombre
            };

            _context.Sum.Add(sum);
            _context.SaveChanges();
        }

        public void EditarSum(SumViewModel vm)
        {
            var existente = _context.Sum.FirstOrDefault(s => s.Id == vm.Id);
            if (existente == null)
                throw new Exception("Sum no encontrada");

            existente.Nombre = vm.Nombre;
            _context.SaveChanges();
        }

        public void EliminarSum(int id)
        {
            var sum = _context.Sum.Find(id);
            if (sum != null)
            {
                _context.Sum.Remove(sum);
                _context.SaveChanges();
            }
        }
    }
}
