using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services.Interfaces;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class ExpensaController : Controller
    {
        private readonly IExpensaService _expensaService;

        public ExpensaController(IExpensaService expensaService)
        {
            _expensaService = expensaService;
        }

        public async Task<IActionResult> Index(int Id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = await _expensaService.ObtenerDatosConsorcio(Id, usuarioId);

            if (consorcio == null)
                return NotFound();
            ViewBag.NombreConsorcio = consorcio.ConsorcioNombre;
            ViewBag.IdConsorcio = consorcio.ConsorcioId;
            return View(consorcio);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpensas(int Id)
        {

            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var expensas = await _expensaService.GetExpensasPorMes(Id, usuarioId);
            return Json(new { data = expensas });
        }

    }
}
