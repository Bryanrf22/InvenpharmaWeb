using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Inventario
    {
        private CD_Inventario objCapaDatos = new CD_Inventario();

        public List<Inventario> ListarInventario()
        {
            return objCapaDatos.ListarInventario();
        }

        public int AgregarInventario(Inventario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del producto no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion del producto no puede estar vacía";
            }
            else if (obj.TipoID == 0)
            {
                Mensaje = "Debe Seleccionar un tipo";
            }
            else if (obj.ProveedorID == 0)
            {
                Mensaje = "Debe Seleccionar un proveedor";
            }
            else if (obj.CategoriaID == 0)
            {
                Mensaje = "Debe Seleccionar una categoria";
            }
            else if (obj.PrecioUnitario <= 0)
            {
                Mensaje = "El precio no puede ser menor o igual a 0";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.AgregarProducto(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarInventario(Inventario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del producto no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion del producto no puede estar vacía";
            }
            else if (obj.TipoID == 0)
            {
                Mensaje = "Debe Seleccionar un tipo";
            }
            else if (obj.ProveedorID == 0)
            {
                Mensaje = "Debe Seleccionar un proveedor";
            }
            else if (obj.CategoriaID == 0)
            {
                Mensaje = "Debe Seleccionar una categoria";
            }
            else if (obj.PrecioUnitario <= 0)
            {
                Mensaje = "El precio no puede ser menor o igual a 0";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.EditarProducto(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarInventario(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarProducto(id, out Mensaje);
        }

        public bool GuardarImagen(int id, Inventario obj, out string Mensaje)
        {
            return objCapaDatos.GuardarDatosImagen(id, obj, out Mensaje);
        }
    }
}