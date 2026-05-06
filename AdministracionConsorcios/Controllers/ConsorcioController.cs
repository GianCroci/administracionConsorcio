using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdministracionConsorcios.Controllers
{
    [Authorize]
    public class ConsorcioController : Controller
    {
        // GET: ConsorcioController
        public ActionResult Index()
        {
            return View();
        }

        
    }
}
