using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var consorcio = await _expensaService.ObtenerDatosConsorcio(Id);

            if (consorcio == null)
                return NotFound();

            return View(consorcio);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpensas(int Id)
        {


            var expensas = await _expensaService.GetExpensasPorMes(Id);
            return Json(new { data = expensas });
        }

    }
}
