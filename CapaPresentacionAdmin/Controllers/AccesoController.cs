using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    [AllowAnonymous]
    public class AccesoController : Controller
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public AccesoController(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            // Exponer la URL de la tienda a la vista para redirecciones (si está configurada)
            ViewBag.TiendaUrl = _config["TiendaUrl"]?.ToString()?.TrimEnd('/');
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
        public async Task<IActionResult> Index(string correo, string clave)
        {
            Usuario? objUsuario = null;
            objUsuario = new CN_Usuarios().ListarUsuarios().Where(u => u.Correo == correo && u.clave == CN_recursos.ConvertirSHA256(clave)).FirstOrDefault();


            if (objUsuario == null)
            {
                ViewBag.Error = "Correo o Clave incorrectos";
                return View();
            }

            if (!objUsuario.activo)
            {
                ViewBag.Error = "El usuario no está activo. Contacte al administrador.";
                return View();
            }

            if (!objUsuario.administrador)
            {
                // var tiendaUrl = _config["TiendaUrl"];
                // if (!string.IsNullOrWhiteSpace(tiendaUrl))
                // {
                //     tiendaUrl = tiendaUrl.TrimEnd('/');
                //     var target = tiendaUrl + "/Home/Index";
                //     // Comprobar si la tienda responde antes de redirigir; si no, usar fallback local
                //     if (await IsUrlReachableAsync(target))
                //     {
                //         return Redirect(target);
                //     }
                //     else
                //     {
                //         return RedirectToAction("Index", "Home");
                //     }
                // }
                // else
                // {
                //     return RedirectToAction("Index", "Home");
                // }

                ViewBag.Error = "El usuario no tiene permisos de administrador.";
                return View();
            }

            if (objUsuario.reestablecer == true)
            {
                TempData["UsuarioID"] = objUsuario.UsuarioID;
                return RedirectToAction("CambiarClave", "Acceso");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, objUsuario.Correo ?? string.Empty),
                new Claim(ClaimTypes.Email, objUsuario.Correo ?? string.Empty),
                new Claim("UsuarioID", objUsuario.UsuarioID.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            ViewBag.Error = null;
            // Redirigir al Home del Admin
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public IActionResult CambiarClave(string idUsuario, string claveActual, string claveNueva, string claveConfirmar)
        {
            Usuario? objUsuario = null;
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
            Usuario? objUsuario = null;
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


        public async Task<IActionResult> CerrarSesion()
        {
            // Cerrar sesión (remover cookie)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Acceso");
        }

        // Comprueba si una URL responde antes de redirigir (acepta certificados auto-firmados)
        private async Task<bool> IsUrlReachableAsync(string url)
        {
            try
            {
                using var handler = new HttpClientHandler();
                // En entornos de desarrollo aceptar certificados autofirmados
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                using var client = new HttpClient(handler)
                {
                    Timeout = TimeSpan.FromSeconds(2)
                };

                var request = new HttpRequestMessage(HttpMethod.Head, url);
                var response = await client.SendAsync(request);

                // Si el servidor no admite HEAD, intentar GET
                if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
                {
                    response = await client.GetAsync(url);
                }

                int code = (int)response.StatusCode;
                return code >= 200 && code < 400;
            }
            catch
            {
                return false;
            }
        }





    }
}
