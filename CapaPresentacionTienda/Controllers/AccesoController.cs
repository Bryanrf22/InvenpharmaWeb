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
            // validar parámetros básicos
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(clave))
            {
                ViewBag.Error = "Debe ingresar correo y contraseña";
                return View();
            }

            Usuario? objeto = cnUsuarios.ListarClientes()
                .Where(u => u.Correo == correo && u.clave == CN_recursos.ConvertirSHA256(clave))
                .FirstOrDefault();

            if (objeto == null)
            {
                ViewBag.Error = "Correo o contraseña incorrecta";
                return View();
            }

            // Verificar que el usuario esté activo
            if (!objeto.activo)
            {
                ViewBag.Error = "Usuario inactivo. Contacte al administrador.";
                return View();
            }

            if (objeto.reestablecer)
            {
                TempData["UsuarioID"] = objeto.UsuarioID;
                return RedirectToAction("CambiarClave", "Acceso");
            }

            // Guardar información mínima en sesión
            HttpContext.Session.SetInt32("UsuarioID", objeto.UsuarioID);
            HttpContext.Session.SetString("Correo", objeto.Correo ?? string.Empty);
            HttpContext.Session.SetString("Administrador", objeto.administrador.ToString());

            // Redirigir al home de la tienda
            return RedirectToAction("Index", "Tienda");
        }


        [HttpPost]
        public IActionResult RestablecerClave(string correo)
        {
            Usuario? objUsuario = null;
            objUsuario = new CN_Usuarios().ListarClientes().Where(u => u.Correo == correo).FirstOrDefault();

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




        [HttpPost]
        public IActionResult CambiarClave(int idUsuario, string claveNueva, string confirmarClave)
        {
            if (claveNueva != confirmarClave)
            {
                TempData["UsuarioID"] = idUsuario;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            int idUsr = idUsuario;

            if (idUsr == 0)
            {
                ViewBag.Error = "Usuario no válido";
                return View();
            }

            // Cambiar la contraseña
            string mensaje = string.Empty;
            bool resultado = new CN_Usuarios().CambiarClave(idUsr, CN_recursos.ConvertirSHA256(claveNueva), out mensaje);

            if (resultado)
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