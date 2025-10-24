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
            Usuario? objUsuario = null;

            objUsuario = new CN_Usuarios().ListarUsuarios().Where(u => u.Correo == correo && u.clave == CN_recursos.ConvertirSHA256(clave)).FirstOrDefault();

            if (objUsuario == null)
            {
                ViewBag.Error = "Correo o Clave incorrectos";
                return View();
            }
            else
            {
                if (objUsuario.reestablecer == true)
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



        [HttpPost]
        public IActionResult CambiarClave(string idUsuario, string claveActual, string claveNueva, string claveConfirmar)
        {
            Usuario objUsuario = new Usuario();
            if (!int.TryParse(idUsuario, out int idUsr))
            {
                ViewBag.Error = "Id de usuario inválido";
                return View();
            }

            objUsuario = new CN_Usuarios().ListarUsuarios().Where(u => u.UsuarioID == idUsr).FirstOrDefault();

            if (objUsuario == null)
            {
                ViewBag.Error = "Usuario no encontrado";
                return View();
            }

            if (objUsuario.clave != CN_recursos.ConvertirSHA256(claveActual))
            {
                TempData["UsuarioID"] = idUsuario;
                ViewData["vClaveActual"] = "";

                ViewBag.Error = "La clave actual no es correcta";
                return View();
            }
            else if (claveNueva != claveConfirmar)
            {
                TempData["UsuarioID"] = idUsuario;
                ViewData["vClaveActual"] = claveActual;
                ViewBag.Mensaje = "Las nuevas claves no coinciden";
                return View();
            }

            ViewData["vClaveActual"] = "";

            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave(idUsr, claveNueva, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                TempData["UsuarioID"] = idUsuario;
                ViewBag.Error = mensaje;
                return View();
            }


        }


        [HttpPost]
        public IActionResult ReestablecerClave(string correo)
        {
            Usuario objUsuario = new Usuario();
            objUsuario = new CN_Usuarios().ListarUsuarios().Where(u => u.Correo == correo).FirstOrDefault();

            if (objUsuario == null)
            {
                ViewBag.Error = "No se encontró un usuario con ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablecerClave(objUsuario.UsuarioID, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
            }

            return View();
        }





    }
}
