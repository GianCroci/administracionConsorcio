using Model;
using DTOs.ViewModels;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ISumService
    {
        List<Sum> ObtenerSums(int idConsorcio);
        Sum ObtenerSumPorId(int id);
        void AgregarSum(SumViewModel sumVm);
        void EditarSum(SumViewModel sumVm);
        void EliminarSum(int id);
    }
}
