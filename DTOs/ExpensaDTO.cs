using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ExpensaDTO
    {
        public int ConsorcioId { get; set; }
        public string ConsorcioNombre { get; set; } = string.Empty;

        public string Año { get; set; } = string.Empty;
        public string Mes { get; set; } = string.Empty;
        public decimal GastoMes { get; set; }
        public int CantidadUnidades { get; set; }
        public decimal MontoXUnidad { get; set; }

        public string MesNombre(int mes)
        {
            return mes switch
            {
                1 => "Enero",
                2 => "Febrero",
                3 => "Marzo",
                4 => "Abril",
                5 => "Mayo",
                6 => "Junio",
                7 => "Julio",
                8 => "Agosto",
                9 => "Septiembre",
                10 => "Octubre",
                11 => "Noviembre",
                12 => "Diciembre",
                _ => "Mes inválido"
            };
        }
    }
}
