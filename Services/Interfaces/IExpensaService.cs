using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IExpensaService
    {
        Task<List<ExpensaDTO>> GetExpensasPorMes(int consorcioId);

        Task<ExpensaDTO> ObtenerDatosConsorcio(int consorcioId);
    }
}
