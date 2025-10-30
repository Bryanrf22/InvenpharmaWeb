using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Security.Cryptography;
using System.IO;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> lista = new CN_Categoria().ListarCategoria();
            return Json(new { data = lista });
        }

        [HttpGet]
        public JsonResult ListarTipos()
        {
            List<Tipo> lista = new CN_Tipo().ListarTipo();
            return Json(new { data = lista });
        }

        [HttpPost]
        public JsonResult ListarProductos(int idCategoria, int idTipo)
        {
            List<Inventario> lista = new List<Inventario>();
            bool convercion;

            lista = new CN_Inventario().ListarInventario().Select(p => new Inventario()
            {
                InventarioID = p.InventarioID,
                Nombre = p.Nombre,
                TipoID = p.TipoID,
                ProveedorID = p.ProveedorID,
                CategoriaID = p.CategoriaID,
                Descripcion = p.Descripcion,
                PrecioUnitario = p.PrecioUnitario,
                Stock = p.Stock,
                rutaimagen = p.rutaimagen,
                Base64 = CN_recursos.ConvertirBase64(Path.Combine(p.rutaimagen, p.nombreimagen), out convercion),
                Extension = Path.GetExtension(p.nombreimagen)
            }).Where(i => i.CategoriaID == (idCategoria == 0 ? i.CategoriaID : idCategoria) &&
                i.TipoID == (idTipo == 0 ? i.TipoID : idTipo) && i.Stock > 0).ToList();
            var jsonResult = Json(new { data = lista });
            return jsonResult;
        }


    }
}
