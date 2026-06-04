using DTOs.ViewModels;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IGastoService
    {
        List<Gasto> ObtenerGastos(int idConsorcio);
        Gasto ObtenerGastoPorId(int id);
        void AgregarGasto(GastoViewModel gastoVm, int usuarioId, string archivoComprobante);
        void EditarGasto(GastoViewModel gastoVm, string nuevoArchivoComprobante);
        void EliminarGasto(int id);
        List<TipoGasto> ObtenerTiposGasto();
    }
}
