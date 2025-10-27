using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        private CN_Usuarios cnUsuarios = new CN_Usuarios();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegistroCliente()
        {
            return View();
        }

        public IActionResult RestablecerClave()
        {
            return View();
        }

        public IActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistroCliente(Usuario objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if (objeto.clave != objeto.confirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            resultado = new CN_Usuarios().AgregarCliente(objeto, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Index(string correo, string clave)
        {
            Usuario objeto = null;
            objeto = cnUsuarios.ListarClientes().Where(u => u.Correo == correo && u.clave == CN_recursos.ConvertirSHA256(clave)).FirstOrDefault();

            if (objeto == null)
            {
                ViewBag.Error = "Correo o contraseña incorrecta";
                return View();
            }
            else
            {
                if (objeto.reestablecer)
                {
                    TempData["UsuarioID"] = objeto.UsuarioID;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    HttpContext.Session.SetInt32("UsuarioID", objeto.UsuarioID);
                    HttpContext.Session.SetString("Correo", objeto.Correo);
                    HttpContext.Session.SetString("Administrador", objeto.administrador.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }
    }
}