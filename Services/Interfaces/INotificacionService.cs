using DTOs.ViewModels;
using Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface INotificacionService
    {
        List<Notificacion> ObtenerNotificaciones(int idConsorcio);
        Notificacion ObtenerNotificacionPorId(int id);
        void AgregarNotificacion(NotificacionViewModel vm, int usuarioId);
        void EditarNotificacion(NotificacionViewModel vm);
        void EliminarNotificacion(int id);
        Task EnviarNotificacionAsync(int id, IEmailService emailService);
    }
}