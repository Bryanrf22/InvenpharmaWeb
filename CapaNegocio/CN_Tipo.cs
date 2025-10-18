using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Tipo
    {
        private CD_Tipo objCapaDatos = new CD_Tipo();

        public List<Tipo> ListarTipo()
        {
            return objCapaDatos.Listartipo();
        }

        public int AgregarTipo(Tipo obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Tipo no puede estar vacío";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.AgregarTipo(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarTipo(Tipo obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Tipo no puede estar vacío";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.EditarTipo(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarTipo(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarTipo(id, out Mensaje);
        }
    }
}