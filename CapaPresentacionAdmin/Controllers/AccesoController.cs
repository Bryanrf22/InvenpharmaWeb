using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CambiarClave()
        {
            return View();
        }

        public IActionResult ReestablecerClave()
        {
            return View();
        }


    }
}
