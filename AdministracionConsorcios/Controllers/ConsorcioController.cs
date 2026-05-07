using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class ConsorcioController : Controller
    {

        private Servicio.IConsorcioServicio _consorcioServicio;

        public ConsorcioController(Servicio.IConsorcioServicio consorcioServicio)
        {
            _consorcioServicio = consorcioServicio;
        }
        
        public IActionResult Index()
        {
            List<Consorcio> consorcios = _consorcioServicio.ObtenerConsorcios();
            return View(consorcios);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            var consorcio = _consorcioServicio.ObtenerConsorcios().FirstOrDefault(h => h.Id == id);
            if (consorcio == null)
            {
                return NotFound();
            }
            return View(consorcio);
        }

        public IActionResult EliminarConsorcio(int id)
        {
            _consorcioServicio.EliminarConsorcio(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CrearConsorcio(Model.Consorcio consorcio)
        {
            if (ModelState.IsValid)
            {
                _consorcioServicio.AgregarConsorcio(consorcio);
                return RedirectToAction("Index");
            }
            return View(consorcio);
        }

        public IActionResult EditarConsorcio(Model.Consorcio consorcio)
        {
            if (ModelState.IsValid)
            {
                _consorcioServicio.EditarConsorcio(consorcio);
                return RedirectToAction("Index");
            }

            return View("Editar", consorcio);
        }

        
    }
}
