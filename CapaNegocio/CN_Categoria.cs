using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDatos = new CD_Categoria();

        public List<Categoria> ListarCategoria()
        {
            return objCapaDatos.ListarCategoria();
        }

        public int AgregarCategoria(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre de la Categoria no puede estar vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.AgregarCategoria(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarCategoria(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre de la Categoria no puede estar vacío";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.EditarCategoria(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarCategoria(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarCategoria(id, out Mensaje);
        }
    }
}