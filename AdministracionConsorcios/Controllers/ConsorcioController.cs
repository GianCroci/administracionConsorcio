using DTOs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services.Interfaces;
using System.Security.Claims;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class ConsorcioController : Controller
    {

        private IConsorcioService _consorcioService;

        public ConsorcioController(IConsorcioService consorcioService)
        {
            _consorcioService = consorcioService;
        }

        public IActionResult Index()
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            List<Consorcio> consorcios = _consorcioService.ObtenerConsorcios(usuarioId);
            return View(consorcios);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Provincias = _consorcioService.obtenerProvincias();
            return View();
        }

        public IActionResult Editar(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(h => h.Id == id);
            if (consorcio == null)
            {
                return NotFound();
            }

            ViewBag.Provincias = _consorcioService.obtenerProvincias();

            var consorcioVm = new ConsorcioViewModel
            {
                Id = consorcio.Id,
                Nombre = consorcio.Nombre,
                Ciudad = consorcio.Ciudad,
                Calle = consorcio.Calle,
                Altura = consorcio.Altura,
                DiaVencimientoExpensas = consorcio.DiaVencimientoExpensas,
                IdProvincia = consorcio.IdProvincia
            };

            return View(consorcioVm);
        }

        public IActionResult Eliminar(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
            var consorcio = _consorcioService.ObtenerConsorcios(usuarioId).FirstOrDefault(h => h.Id == id);
            var consorcioVm = new ConsorcioViewModel
            {
                Id = consorcio.Id,
                Nombre = consorcio.Nombre,
            };
            return View(consorcioVm);
        }

        [HttpPost]
        public async Task<IActionResult> CrearConsorcio(ConsorcioViewModel consorciovm,string accion)
        {         
            if (!ModelState.IsValid)
            {
                ViewBag.Provincias = _consorcioService.obtenerProvincias();
                return View("Crear", consorciovm);
            }

            try
            {
                var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

                await _consorcioService.AgregarConsorcio(consorciovm, usuarioId);

                switch (accion)
                {
                    case "guardar":
                        return RedirectToAction("Index");

                    case "guardar_y_nuevo":
                        return RedirectToAction("Crear");

                    /*case "guardar_y_unidad":
                       return RedirectToAction("CrearUnidad", new {});*/

                    default:
                        return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo obtener la ubicación del consorcio. Verificá la dirección.";
                ViewBag.Provincias = _consorcioService.obtenerProvincias();
                return View("Crear",consorciovm);
            }
        }

        public async Task<IActionResult> EditarConsorcio(ConsorcioViewModel consorcioVm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Provincias = _consorcioService.obtenerProvincias();
                return View("Editar", consorcioVm);
            }
            try
            {
                await _consorcioService.EditarConsorcio(consorcioVm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo obtener la ubicación del consorcio. Verificá la dirección.";
                ViewBag.Provincias = _consorcioService.obtenerProvincias();
                return View("Editar", consorcioVm);
            }
        }

        public IActionResult EliminarConsorcio(int id)
        {
            _consorcioService.EliminarConsorcio(id);
            return RedirectToAction("Index");
        }

    }
}
