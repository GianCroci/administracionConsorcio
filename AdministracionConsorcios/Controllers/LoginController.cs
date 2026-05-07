using DTOs.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services.Interfaces;
using System.Security.Claims;

namespace AdministracionConsorcios.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public LoginController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        [HttpGet]
        public IActionResult Ingresar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ingresar(string Email, string Password, string returnUrl = null)
        {
            var usuario = await _usuarioService.Login(Email, Password);
            if (usuario != null)
            {
                HttpContext.Session.SetString("usuario", usuario.Email);

                var claims = new List<Claim>
        {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim("UsuarioId", usuario.Id.ToString())
        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                      new ClaimsPrincipal(claimsIdentity));

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Consorcio");
            }
            ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
            return View();
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(RegistroViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                await _usuarioService.Registrar(vm);
                return RedirectToAction("Ingresar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("Ingresar");
        }
    }
}
