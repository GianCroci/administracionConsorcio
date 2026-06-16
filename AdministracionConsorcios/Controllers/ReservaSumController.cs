using DTOs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class ReservaSumController : Controller
    {
        private readonly IReservaSumService _reservaSumService;
        private readonly ISumService _sumService;

        public ReservaSumController(IReservaSumService reservaSumService, ISumService sumService)
        {
            _reservaSumService = reservaSumService;
            _sumService = sumService;
        }

        public IActionResult Index(int id)
        {
            var sum = _sumService.ObtenerSumPorId(id);
            if (sum == null)
                return NotFound();

            var reservas = _reservaSumService.ObtenerReservas(id);
            ViewBag.IdSum = id;
            ViewBag.NombreSum = sum.Nombre;
            ViewBag.IdConsorcio = sum.IdConsorcio;
            ViewBag.NombreConsorcio = sum.Consorcio.Nombre;
            return View(reservas);
        }

        [HttpGet]
        public IActionResult Crear(int id)
        {
            var sum = _sumService.ObtenerSumPorId(id);
            if (sum == null)
                return NotFound();

            var vm = new ReservaSumViewModel
            {
                IdSum = id,
                NombreSum = sum.Nombre,
                FechaReserva = DateTime.Now
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult CrearReserva(ReservaSumViewModel reservaVm, string accion)
        {
            if (!ModelState.IsValid)
            {
                var sum = _sumService.ObtenerSumPorId(reservaVm.IdSum);
                reservaVm.NombreSum = sum?.Nombre;
                return View("Crear", reservaVm);
            }
            try
            {
                var sumValidar = _sumService.ObtenerSumPorId(reservaVm.IdSum);
                if (sumValidar == null)
                    return NotFound();

                var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);
                _reservaSumService.AgregarReserva(reservaVm, usuarioId);
                TempData["Exito"] = $"Reserva del {reservaVm.FechaReserva:dd/MM/yyyy} creada con éxito";

                if (accion == "guardar_y_nuevo")
                    return RedirectToAction("Crear", new { id = reservaVm.IdSum });

                return RedirectToAction("Index", new { id = reservaVm.IdSum });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                var sum = _sumService.ObtenerSumPorId(reservaVm.IdSum);
                reservaVm.NombreSum = sum?.Nombre;
                return View("Crear", reservaVm);
            }


        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var reserva = _reservaSumService.ObtenerReservaPorId(id);
            if (reserva == null)
                return NotFound();

            var vm = new ReservaSumViewModel
            {
                Id = reserva.Id,
                IdSum = reserva.IdSum,
                NombreSum = reserva.Sum.Nombre,
                FechaReserva = reserva.FechaReserva,
                Turno = (int)reserva.Turno,
                Comentarios = reserva.Comentarios,
                EntregoCorrectamente = reserva.EntregoCorrectamente,
                IdUsuario = reserva.IdUsuario,
                NombreUsuario = reserva.UsuarioQueReserva.Email
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditarReserva(ReservaSumViewModel reservaVm)
        {
            if (!ModelState.IsValid)
            {
                return View("Editar", reservaVm);
            }

            try
            {

            var reservaValidar = _reservaSumService.ObtenerReservaPorId(reservaVm.Id);
            if (reservaValidar == null)
                return NotFound();

            _reservaSumService.EditarReserva(reservaVm);
            TempData["Exito"] = $"Reserva del {reservaVm.FechaReserva:dd/MM/yyyy} actualizada con éxito";
            return RedirectToAction("Index", new { id = reservaVm.IdSum });

            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("Editar", reservaVm);
            }

        }

        public IActionResult Eliminar(int id)
        {
            var reserva = _reservaSumService.ObtenerReservaPorId(id);
            if (reserva == null)
                return NotFound();

            var vm = new ReservaSumViewModel
            {
                Id = reserva.Id,
                IdSum = reserva.IdSum,
                NombreSum = reserva.Sum.Nombre,
                FechaReserva = reserva.FechaReserva,
                Turno = (int)reserva.Turno,
                NombreUsuario = reserva.UsuarioQueReserva.Email
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EliminarReserva(int id, int idSum)
        {
            var reserva = _reservaSumService.ObtenerReservaPorId(id);
            if (reserva == null)
                return NotFound();

            _reservaSumService.EliminarReserva(id);
            TempData["Exito"] = $"Reserva del {reserva.FechaReserva:dd/MM/yyyy} eliminada con éxito";
            return RedirectToAction("Index", new { id = idSum });
        }
    }
}
