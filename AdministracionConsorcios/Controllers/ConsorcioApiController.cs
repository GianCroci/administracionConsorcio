using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    [Route("api/consorcio")]
    [ApiController]
    public class ConsorcioApiController : ControllerBase
    {
        private readonly IConsorcioService _consorcioService;

        public ConsorcioApiController(IConsorcioService consorcioService)
        {
            _consorcioService = consorcioService;
        }

        [HttpGet("listado-mapa")]
        public IActionResult ListadoMapa()
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

            var consorcios = _consorcioService.ObtenerConsorcios(usuarioId);

            return Ok(consorcios.Select(c => new
            {
                id = c.Id,
                nombre = c.Nombre,
                ciudad = c.Ciudad,
                calle = c.Calle,
                altura = c.Altura,
                latitud = c.Latitud,
                longitud = c.Longitud
            }));
        }
    }
}
