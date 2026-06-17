using DTOs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class SumController : Controller
    {
        private readonly ISumService _sumService;
        private readonly IConsorcioService _consorcioService;

        public SumController(ISumService sumService, IConsorcioService consorcioService)
        {
            _sumService = sumService;
            _consorcioService = consorcioService;
        }

        // GET: /Sum/Index?idConsorcio=5
        public IActionResult Index(int idConsorcio)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == idConsorcio);
            if (consorcio == null)
                return NotFound();

            var sums = _sumService.ObtenerSums(idConsorcio);
            ViewBag.IdConsorcio = idConsorcio;
            ViewBag.NombreConsorcio = consorcio.Nombre;
            return View(sums);
        }

        // GET: /Sum/Crear?idConsorcio=5
        [HttpGet]
        public IActionResult Crear(int idConsorcio)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == idConsorcio);
            if (consorcio == null)
                return NotFound();

            var vm = new SumViewModel
            {
                IdConsorcio = idConsorcio,
                NombreConsorcio = consorcio.Nombre
            };
            return View(vm);
        }

        // POST: /Sum/CrearSum
        [HttpPost]
        public IActionResult CrearSum(SumViewModel vm, string accion)
        {
            if (!ModelState.IsValid)
                return View("Crear", vm);

            _sumService.AgregarSum(vm);
            TempData["Exito"] = $"Sum \"{vm.Nombre}\" creada con éxito";

            if (accion == "guardar_y_nuevo")
                return RedirectToAction("Crear", new { idConsorcio = vm.IdConsorcio });

            return RedirectToAction("Index", new { idConsorcio = vm.IdConsorcio });
        }

        // GET: /Sum/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var sum = _sumService.ObtenerSumPorId(id);
            if (sum == null)
                return NotFound();

            var vm = new SumViewModel
            {
                Id = sum.Id,
                IdConsorcio = sum.IdConsorcio,
                NombreConsorcio = sum.Consorcio.Nombre,
                Nombre = sum.Nombre,
                CantidadReservas = sum.Reservas.Count
            };
            return View(vm);
        }

        // POST: /Sum/EditarSum
        [HttpPost]
        public IActionResult EditarSum(SumViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Editar", vm);

            _sumService.EditarSum(vm);
            TempData["Exito"] = $"Sum \"{vm.Nombre}\" actualizada con éxito";
            return RedirectToAction("Index", new { idConsorcio = vm.IdConsorcio });
        }

        // GET: /Sum/Eliminar/5
        public IActionResult Eliminar(int id)
        {
            var sum = _sumService.ObtenerSumPorId(id);
            if (sum == null)
                return NotFound();

            var vm = new SumViewModel
            {
                Id = sum.Id,
                IdConsorcio = sum.IdConsorcio,
                Nombre = sum.Nombre,
                CantidadReservas = sum.Reservas.Count
            };
            return View(vm);
        }

        // POST: /Sum/EliminarSum
        [HttpPost]
        public IActionResult EliminarSum(int id, int idConsorcio)
        {
            var sum = _sumService.ObtenerSumPorId(id);
            _sumService.EliminarSum(id);
            TempData["Exito"] = $"Sum \"{sum?.Nombre}\" eliminada con éxito";
            return RedirectToAction("Index", new { idConsorcio = idConsorcio });
        }
    }
}
