using Microsoft.AspNetCore.Mvc;

using CapaEntidad;
using CapaNegocio;

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

        [HttpPost]
        public IActionResult Index(string correo, string clave)
        {
            Usuario objUsuario = new Usuario();
            
            objUsuario=new CN_Usuarios().ListarUsuarios().Where(u => u.Correo == correo && u.clave == CN_recursos.ConvertirSHA256(clave)).FirstOrDefault();

            if (objUsuario == null)
            {
                ViewBag.Error = "Correo o Clave incorrectos";
                return View();
            }
            else
            {
                if (objUsuario.reestablecer)
                {
                    TempData["UsuarioID"] = objUsuario.UsuarioID;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else

                {
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Home");
                }

            }
        }



    }
}
