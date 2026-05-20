using DTOs.ViewModels;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IConsorcioService
    {
        List<Consorcio> ObtenerConsorcios(int usuarioId);
        Task AgregarConsorcio(ConsorcioViewModel consorcioVm, int usuarioId);
        void EliminarConsorcio(int id);
        Task EditarConsorcio(ConsorcioViewModel consorcioVm);
        List<Provincia> obtenerProvincias();
    }
}
