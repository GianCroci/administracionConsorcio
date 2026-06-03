using Model;

namespace Services.Interfaces
{
    public interface IUnidadService
    {
        List<Unidad> ObtenerUnidades(int idConsorcio, int usuarioId);
        Consorcio ObtenerConsorcio(int idConsorcio, int usuarioId);
        Unidad ObtenerUnidad(int id, int usuarioId);
        Task AgregarUnidad(Unidad unidad, int usuarioId);
        Task EditarUnidad(Unidad unidad, int usuarioId);
        void EliminarUnidad(int id, int usuarioId);
    }
}