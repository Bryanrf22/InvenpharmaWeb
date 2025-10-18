using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Proveedor
    {
        private CD_Proveedor objCapaDatos = new CD_Proveedor();

        public List<Proveedor> ListarProveedor()
        {
            return objCapaDatos.ListarProveedor();
        }

        public int AgregarProveedor(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Proveedor no puede estar vacío";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.AgregarProveedor(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarProveedor(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Proveedor no puede estar vacío";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.EditarProveedor(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarProveedor(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarProveedor(id, out Mensaje);
        }
    }
}