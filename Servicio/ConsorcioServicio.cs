using Data;
using Model;


namespace Servicio
{
    public interface IConsorcioServicio
    {
        List<Consorcio> ObtenerConsorcios();
        void AgregarConsorcio(Consorcio consorcio);
        void EliminarConsorcio(int id);
        void EditarConsorcio(Consorcio consorcio);
    }

    public class ConsorcioServicio : IConsorcioServicio
    {
        private readonly ConsorcioContext _context;
        public ConsorcioServicio(ConsorcioContext context)
        {
            _context = context;
        }

        public void AgregarConsorcio(Consorcio consorcio)
        {
            _context.Consorcios.Add(consorcio);
            _context.SaveChanges();
        }

        public void EditarConsorcio(Consorcio consorcio)
        {
            _context.Consorcios.Update(consorcio);
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

        public List<Consorcio> ObtenerConsorcios()
        {
            return _context.Consorcios.ToList();
        }
    }
}
