using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services;
using Services.Interfaces;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class UnidadController : Controller
    {
        private readonly IUnidadService _unidadService;
        private readonly EmailService _emailService;

        public UnidadController(IUnidadService unidadService, EmailService emailService)
        {
            _unidadService = unidadService;
            _emailService = emailService;
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

                var consorcio = _unidadService.ObtenerConsorcio(unidad.IdConsorcio, usuarioId);

                var destinatarios = new List<string> { unidad.EmailPropietario };

                var asunto = "Nueva unidad registrada";

                var cuerpoHtml = $@"
                    <h2>Unidad registrada correctamente</h2>

                    <p>Hola {unidad.NombrePropietario} {unidad.ApellidoPropietario},</p>

                    <p>Se registró una nueva unidad a tu nombre en el sistema de Administración de Consorcios.</p>

                    <ul>
                        <li><strong>Consorcio:</strong> {consorcio?.Nombre}</li>
                        <li><strong>Unidad:</strong> {unidad.Nombre}</li>
                        <li><strong>Superficie:</strong> {unidad.Superficie} m²</li>
                    </ul>

                    <p>Saludos.</p>
                ";

                try
                {
                    await _emailService.EnviarAsync(destinatarios, asunto, cuerpoHtml);
                    TempData["Exito"] = $"Unidad \"{unidad.Nombre}\" creada con éxito y se envió la notificación por mail";
                }
                catch
                {
                    TempData["Exito"] = $"Unidad \"{unidad.Nombre}\" creada con éxito, pero no se pudo enviar el mail";
                }

                switch (accion)
                {
                    case "guardar":
                        return RedirectToAction("Index", new { idConsorcio = unidad.IdConsorcio });

                    case "guardar_y_nuevo":
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

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

            var unidad = _unidadService.ObtenerUnidad(id, usuarioId);

            if (unidad == null)
            {
                return NotFound();
            }

            ViewBag.IdConsorcio = unidad.IdConsorcio;
            ViewBag.NombreConsorcio = unidad.Consorcio.Nombre;

            return View(unidad);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUnidad(Unidad unidad)
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

                return View("Editar", unidad);
            }

            try
            {
                var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

                await _unidadService.EditarUnidad(unidad, usuarioId);
                TempData["Exito"] = $"Unidad \"{unidad.Nombre}\" actualizada con éxito";

                return RedirectToAction("Index", new { idConsorcio = unidad.IdConsorcio });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var usuarioIdError = int.Parse(User.FindFirst("UsuarioId").Value);
                var consorcioError = _unidadService.ObtenerConsorcio(unidad.IdConsorcio, usuarioIdError);

                ViewBag.IdConsorcio = unidad.IdConsorcio;
                ViewBag.NombreConsorcio = consorcioError?.Nombre;

                return View("Editar", unidad);
            }
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

            var unidad = _unidadService.ObtenerUnidad(id, usuarioId);

            if (unidad == null)
            {
                return NotFound();
            }

            return View(unidad);
        }

        [HttpPost]
        public IActionResult EliminarUnidad(int id, int idConsorcio)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var unidad = _unidadService.ObtenerUnidad(id, usuarioId);

            _unidadService.EliminarUnidad(id, usuarioId);

            TempData["Exito"] = $"Unidad \"{unidad?.Nombre}\" eliminada con éxito";

            return RedirectToAction("Index", new { idConsorcio });
        }
    }
}