using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDatos = new CD_Usuarios();

        public List<Usuario> ListarUsuarios()
        {
            return objCapaDatos.ListarUsuarios();
        }

        public int RegistrarUsuario(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_recursos.GenerarClave();

                string asunto = "Creación de Cuenta";
                string mensajeCorreo = "<h3>Su cuenta fue creada correctamente por un administrador</h3></br><p>Su contraseña temporal para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);

                bool respuesta = CN_recursos.EnviarCorreo(obj.Correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    obj.clave = CN_recursos.ConvertirSHA256(clave);

                    return objCapaDatos.RegistrarUsuario(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo al usuario";
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public bool EditarUsuario(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.EditarUsuario(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarUsuario(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarUsuario(id, out Mensaje);
        }

        public bool CambiarClave(int id, string nuevaclave, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(nuevaclave) || string.IsNullOrWhiteSpace(nuevaclave))
            {
                Mensaje = "La nueva clave no puede estar vacía";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                nuevaclave = CN_recursos.ConvertirSHA256(nuevaclave);
                return objCapaDatos.CambiarClave(id, nuevaclave, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool ReestablecerClave(int id, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = CN_recursos.GenerarClave();
            bool resultado = objCapaDatos.ReestablecerClave(id, CN_recursos.ConvertirSHA256(nuevaclave), out Mensaje);

            if (resultado)
            {
                string asunto = "Reestablecimiento de Contraseña";
                string mensajeCorreo = "<h3>Su contraseña fue reestablecida correctamente</h3></br><p>Su nueva contraseña temporal para acceder ahora es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaclave);
                bool respuesta = CN_recursos.EnviarCorreo(correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo al usuario";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la clave";
                return false;
            }
        }

        public int AgregarCliente(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_recursos.GenerarClave();

                obj.clave = CN_recursos.ConvertirSHA256(clave);
                return objCapaDatos.AgregarCliente(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }
    }
}