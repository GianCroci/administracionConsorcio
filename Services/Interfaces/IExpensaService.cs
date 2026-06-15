using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IExpensaService
    {
        Task<List<ExpensaDTO>> GetExpensasPorMes(int consorcioId, int usuarioId);

        Task<ExpensaDTO> ObtenerDatosConsorcio(int consorcioId, int usuarioId);
    }
}
