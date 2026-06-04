using DTOs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class GastoController : Controller
    {
        private readonly IGastoService _gastoService;
        private readonly IConsorcioService _consorcioService;
        private readonly IWebHostEnvironment _env;

        public GastoController(IGastoService gastoService, IConsorcioService consorcioService, IWebHostEnvironment env)
        {
            _gastoService = gastoService;
            _consorcioService = consorcioService;
            _env = env;
        }

        // GET: /Gasto/Index/5  (5 = idConsorcio)
        public IActionResult Index(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == id);
            if (consorcio == null)
                return NotFound();

            var gastos = _gastoService.ObtenerGastos(id);
            ViewBag.IdConsorcio = id;
            ViewBag.NombreConsorcio = consorcio.Nombre;
            return View(gastos);
        }

        // GET: /Gasto/Crear/5
        [HttpGet]
        public IActionResult Crear(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == id);
            if (consorcio == null)
                return NotFound();

            ViewBag.TiposGasto = _gastoService.ObtenerTiposGasto();
            var vm = new GastoViewModel
            {
                IdConsorcio = id,
                NombreConsorcio = consorcio.Nombre
            };
            return View(vm);
        }

        // POST: /Gasto/CrearGasto
        [HttpPost]
        public IActionResult CrearGasto(GastoViewModel gastoVm, IFormFile archivoComprobante, string accion)
        {
            if (archivoComprobante == null || archivoComprobante.Length == 0)
                ModelState.AddModelError("archivoComprobante", "El comprobante es obligatorio");

            if (!ModelState.IsValid)
            {
                var usuarioId2 = int.Parse(User.FindFirst("UsuarioId").Value);
                var cons = _consorcioService.ObtenerConsorcios(usuarioId2).FirstOrDefault(c => c.Id == gastoVm.IdConsorcio);
                gastoVm.NombreConsorcio = cons?.Nombre;
                ViewBag.TiposGasto = _gastoService.ObtenerTiposGasto();
                return View("Crear", gastoVm);
            }

            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == gastoVm.IdConsorcio);
            if (consorcio == null)
                return NotFound();

            var nombreArchivo = GuardarArchivo(archivoComprobante);
            _gastoService.AgregarGasto(gastoVm, usuarioId, nombreArchivo);

            if (accion == "guardar_y_nuevo")
            {
                TempData["Exito"] = $"Gasto \"{gastoVm.Nombre}\" creado con éxito";
                return RedirectToAction("Crear", new { id = gastoVm.IdConsorcio });
            }

            return RedirectToAction("Index", new { id = gastoVm.IdConsorcio });
        }

        // GET: /Gasto/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var gasto = _gastoService.ObtenerGastoPorId(id);
            if (gasto == null)
                return NotFound();

            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == gasto.IdConsorcio);
            if (consorcio == null)
                return NotFound();

            ViewBag.TiposGasto = _gastoService.ObtenerTiposGasto();
            var vm = new GastoViewModel
            {
                Id = gasto.Id,
                IdConsorcio = gasto.IdConsorcio,
                NombreConsorcio = gasto.Consorcio.Nombre,
                Nombre = gasto.Nombre,
                Descripcion = gasto.Descripcion,
                FechaGasto = gasto.FechaGasto,
                AnioExpensa = gasto.AnioExpensa,
                MesExpensa = gasto.MesExpensa,
                Monto = gasto.Monto,
                IdTipoGasto = gasto.IdTipoGasto,
                ArchivoComprobante = gasto.ArchivoComprobante
            };
            return View(vm);
        }

        // POST: /Gasto/EditarGasto
        [HttpPost]
        public IActionResult EditarGasto(GastoViewModel gastoVm, IFormFile archivoComprobante)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposGasto = _gastoService.ObtenerTiposGasto();
                return View("Editar", gastoVm);
            }

            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == gastoVm.IdConsorcio);
            if (consorcio == null)
                return NotFound();

            string? nuevoArchivo = null;
            if (archivoComprobante != null && archivoComprobante.Length > 0)
                nuevoArchivo = GuardarArchivo(archivoComprobante);

            _gastoService.EditarGasto(gastoVm, nuevoArchivo);
            return RedirectToAction("Index", new { id = gastoVm.IdConsorcio });
        }

        // GET: /Gasto/Eliminar/5
        public IActionResult Eliminar(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var gasto = _gastoService.ObtenerGastoPorId(id);
            if (gasto == null)
                return NotFound();

            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == gasto.IdConsorcio);
            if (consorcio == null)
                return NotFound();

            var vm = new GastoViewModel
            {
                Id = gasto.Id,
                IdConsorcio = gasto.IdConsorcio,
                NombreConsorcio = gasto.Consorcio.Nombre,
                Nombre = gasto.Nombre,
                Monto = gasto.Monto,
                FechaGasto = gasto.FechaGasto
            };
            return View(vm);
        }

        // POST: /Gasto/EliminarGasto
        [HttpPost]
        public IActionResult EliminarGasto(int id, int idConsorcio)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == idConsorcio);
            if (consorcio == null)
                return NotFound();

            _gastoService.EliminarGasto(id);
            return RedirectToAction("Index", new { id = idConsorcio });
        }

        // GET: /Gasto/DescargarComprobante/5
        public IActionResult DescargarComprobante(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var gasto = _gastoService.ObtenerGastoPorId(id);
            if (gasto == null)
                return NotFound();

            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(c => c.Id == gasto.IdConsorcio);
            if (consorcio == null)
                return NotFound();

            var ruta = Path.Combine(_env.WebRootPath, "comprobantes", gasto.ArchivoComprobante);
            if (!System.IO.File.Exists(ruta))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(ruta);
            var contentType = "application/octet-stream";
            return File(bytes, contentType, gasto.ArchivoComprobante);
        }

        private string GuardarArchivo(IFormFile archivo)
        {
            var carpeta = Path.Combine(_env.WebRootPath, "comprobantes");
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            var extension = Path.GetExtension(archivo.FileName);
            var nombreUnico = Guid.NewGuid().ToString() + extension;
            var rutaCompleta = Path.Combine(carpeta, nombreUnico);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                archivo.CopyTo(stream);

            return nombreUnico;
        }
    }
}
