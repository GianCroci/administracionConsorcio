using Model;
using DTOs.ViewModels;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IReservaSumService
    {
        List<ReservaSum> ObtenerReservas(int idSum);
        ReservaSum ObtenerReservaPorId(int id);
        void AgregarReserva(ReservaSumViewModel reservaVm, int usuarioId);
        void EditarReserva(ReservaSumViewModel reservaVm);
        void EliminarReserva(int id);
    }
}
