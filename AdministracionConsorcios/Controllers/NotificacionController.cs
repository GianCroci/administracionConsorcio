using DTOs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class NotificacionController : Controller
    {
        private readonly INotificacionService _notificacionService;
        private readonly IConsorcioService _consorcioService;
        private readonly EmailService _emailService;

        public NotificacionController(INotificacionService notificacionService, IConsorcioService consorcioService, EmailService emailService)
        {
            _notificacionService = notificacionService;
            _consorcioService = consorcioService;
            _emailService = emailService;
        }

        // GET: /Notificacion/Index/5 (5 = idConsorcio)
        public IActionResult Index(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == id);
            if (consorcio == null) return NotFound();

            var notificaciones = _notificacionService.ObtenerNotificaciones(id);
            ViewBag.IdConsorcio = id;
            ViewBag.NombreConsorcio = consorcio.Nombre;
            return View(notificaciones);
        }

        // GET: /Notificacion/Crear/5
        [HttpGet]
        public IActionResult Crear(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == id);
            if (consorcio == null) return NotFound();

            var vm = new NotificacionViewModel
            {
                IdConsorcio = id,
                NombreConsorcio = consorcio.Nombre
            };
            return View(vm);
        }

        // POST: /Notificacion/CrearNotificacion
        [HttpPost]
        public IActionResult CrearNotificacion(NotificacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var usuarioId2 = int.Parse(User.FindFirst("UsuarioId").Value);
                var cons = _consorcioService.ObtenerConsorcios(usuarioId2).FirstOrDefault(c => c.Id == vm.IdConsorcio);
                vm.NombreConsorcio = cons?.Nombre;
                return View("Crear", vm);
            }

            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            _notificacionService.AgregarNotificacion(vm, usuarioId);
            TempData["Exito"] = $"Notificación \"{vm.Titulo}\" creada con éxito";
            return RedirectToAction("Index", new { id = vm.IdConsorcio });
        }

        // GET: /Notificacion/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var notificacion = _notificacionService.ObtenerNotificacionPorId(id);
            if (notificacion == null) return NotFound();

            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == notificacion.IdConsorcio);
            if (consorcio == null) return NotFound();

            var vm = new NotificacionViewModel
            {
                Id = notificacion.Id,
                IdConsorcio = notificacion.IdConsorcio,
                NombreConsorcio = notificacion.Consorcio.Nombre,
                Titulo = notificacion.Titulo,
                Descripcion = notificacion.Descripcion,
                FechaCreacion = notificacion.FechaCreacion,
                FechaEnvio = notificacion.FechaEnvio
            };
            return View("Editar", vm);
        }

        // GET: /Notificacion/Detalle/5
        public IActionResult Detalle(int id) => Editar(id);

        // POST: /Notificacion/EditarNotificacion
        [HttpPost]
        public IActionResult EditarNotificacion(NotificacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposGasto = new List<string>();
                return View("Editar", vm);
            }

            _notificacionService.EditarNotificacion(vm);
            TempData["Exito"] = $"Notificación \"{vm.Titulo}\" actualizada con éxito";
            return RedirectToAction("Index", new { id = vm.IdConsorcio });
        }

        // GET: /Notificacion/Eliminar/5
        public IActionResult Eliminar(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var notificacion = _notificacionService.ObtenerNotificacionPorId(id);
            if (notificacion == null) return NotFound();

            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == notificacion.IdConsorcio);
            if (consorcio == null) return NotFound();

            var vm = new NotificacionViewModel
            {
                Id = notificacion.Id,
                IdConsorcio = notificacion.IdConsorcio,
                NombreConsorcio = notificacion.Consorcio.Nombre,
                Titulo = notificacion.Titulo,
                Descripcion = notificacion.Descripcion,
                FechaCreacion = notificacion.FechaCreacion,
                FechaEnvio = notificacion.FechaEnvio
            };
            return View(vm);
        }

        // POST: /Notificacion/EliminarNotificacion
        [HttpPost]
        public IActionResult EliminarNotificacion(int id, int idConsorcio)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == idConsorcio);
            if (consorcio == null) return NotFound();

            var notificacion = _notificacionService.ObtenerNotificacionPorId(id);
            _notificacionService.EliminarNotificacion(id);
            TempData["Exito"] = $"Notificación \"{notificacion?.Titulo}\" eliminada con éxito";
            return RedirectToAction("Index", new { id = idConsorcio });
        }

        // POST: /Notificacion/Enviar/5
        [HttpPost]
        public async Task<IActionResult> Enviar(int id, int idConsorcio)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == idConsorcio);
            if (consorcio == null) return NotFound();

            try
            {
                await _notificacionService.EnviarNotificacionAsync(id, _emailService);
                TempData["Exito"] = "Notificación enviada correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al enviar: {ex.Message}";
            }

            return RedirectToAction("Index", new { id = idConsorcio });
        }
    }
}