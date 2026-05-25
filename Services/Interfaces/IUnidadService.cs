using Model;

namespace Services.Interfaces
{
    public interface IUnidadService
    {
        List<Unidad> ObtenerUnidades(int idConsorcio, int usuarioId);
        Consorcio ObtenerConsorcio(int idConsorcio, int usuarioId);
        Task AgregarUnidad(Unidad unidad, int usuarioId);
        void EliminarUnidad(int id, int usuarioId);
    }
}