using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IEmailService
    {
        Task EnviarAsync(List<string> emails, string asunto, string cuerpo);
    }
}
