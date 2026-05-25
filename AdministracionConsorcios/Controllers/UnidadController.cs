using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services.Interfaces;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class UnidadController : Controller
    {
        private readonly IUnidadService _unidadService;

        public UnidadController(IUnidadService unidadService)
        {
            _unidadService = unidadService;
        }

        public IActionResult Index(int idConsorcio)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

            var consorcio = _unidadService.ObtenerConsorcio(idConsorcio, usuarioId);

            if (consorcio == null)
            {
                return NotFound();
            }

            ViewBag.IdConsorcio = consorcio.Id;
            ViewBag.NombreConsorcio = consorcio.Nombre;

            var unidades = _unidadService.ObtenerUnidades(idConsorcio, usuarioId);

            return View(unidades);
        }

        [HttpGet]
        public IActionResult Crear(int idConsorcio)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

            var consorcio = _unidadService.ObtenerConsorcio(idConsorcio, usuarioId);

            if (consorcio == null)
            {
                return NotFound();
            }

            ViewBag.IdConsorcio = consorcio.Id;
            ViewBag.NombreConsorcio = consorcio.Nombre;

            return View(new Unidad { IdConsorcio = idConsorcio });
        }

        [HttpPost]
        public async Task<IActionResult> CrearUnidad(Unidad unidad, string accion)
        {
            ModelState.Remove("UsuarioCreador");
            ModelState.Remove("Consorcio");
            ModelState.Remove("IdUsuarioCreador");
            ModelState.Remove("FechaCreacion");

            if (!ModelState.IsValid)
            {
                var usuarioIdError = int.Parse(User.FindFirst("UsuarioId").Value);
                var consorcioError = _unidadService.ObtenerConsorcio(unidad.IdConsorcio, usuarioIdError);

                ViewBag.IdConsorcio = unidad.IdConsorcio;
                ViewBag.NombreConsorcio = consorcioError?.Nombre;

                return View("Crear", unidad);
            }

            try
            {
                var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

                await _unidadService.AgregarUnidad(unidad, usuarioId);

                switch (accion)
                {
                    case "guardar":
                        return RedirectToAction("Index", new { idConsorcio = unidad.IdConsorcio });

                    case "guardar_y_nuevo":
                        TempData["Success"] = "Unidad creada correctamente.";
                        return RedirectToAction("Crear", new { idConsorcio = unidad.IdConsorcio });

                    default:
                        return RedirectToAction("Index", new { idConsorcio = unidad.IdConsorcio });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var usuarioIdError = int.Parse(User.FindFirst("UsuarioId").Value);
                var consorcioError = _unidadService.ObtenerConsorcio(unidad.IdConsorcio, usuarioIdError);

                ViewBag.IdConsorcio = unidad.IdConsorcio;
                ViewBag.NombreConsorcio = consorcioError?.Nombre;

                return View("Crear", unidad);
            }
        }

        public IActionResult EliminarUnidad(int id, int idConsorcio)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

            _unidadService.EliminarUnidad(id, usuarioId);

            return RedirectToAction("Index", new { idConsorcio });
        }
    }
}