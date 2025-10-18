using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Globalization;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        //-----------------------------------------------------CATEGORIA-----------------------------------------------------//
        public IActionResult Categoria()
        {
            return View();
        }

        #region Categoria

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().ListarCategoria();
            Console.WriteLine($"Categorías encontradas: {oLista.Count}");

            //retornar la lista en formato json
            return Json(new { data = oLista });
        }

        [HttpPost]
        public JsonResult GuardarCategoria([FromBody] CapaEntidad.Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.CategoriaID == 0)
            {
                resultado = new CN_Categoria().AgregarCategoria(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().EditarCategoria(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;
            resultado = new CN_Categoria().EliminarCategoria(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        #endregion Categoria

        //-----------------------------------------------------TIPO-----------------------------------------------------//

        public IActionResult Tipo()
        {
            return View();
        }

        #region Tipo

        [HttpGet]
        public JsonResult ListarTipos()
        {
            List<Tipo> oLista = new List<Tipo>();
            oLista = new CN_Tipo().ListarTipo();
            Console.WriteLine($"Tipos encontrados: {oLista.Count}");
            //retornar la lista en formato json
            return Json(new { data = oLista });
        }

        [HttpPost]
        public JsonResult GuardarTipo([FromBody] CapaEntidad.Tipo objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.TipoID == 0)
            {
                resultado = new CN_Tipo().AgregarTipo(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Tipo().EditarTipo(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        [HttpPost]
        public JsonResult EliminarTipo(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;
            resultado = new CN_Tipo().EliminarTipo(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        #endregion Tipo

        //-----------------------------------------------------PRODUCTO-----------------------------------------------------//
        public IActionResult Producto()
        {
            return View();
        }

        #region Producto

        [HttpGet]
        public JsonResult ListarProductos()
        {
            List<Inventario> oLista = new List<Inventario>();
            oLista = new CN_Inventario().ListarInventario();
            Console.WriteLine($"Productos encontrados: {oLista.Count}");
            //retornar la lista en formato json
            return Json(new { data = oLista });
        }

        [HttpPost]
        public JsonResult GuardarProducto([FromForm] string objeto, [FromForm] Microsoft.AspNetCore.Http.IFormFile Imagen)
        {
            object? resultado = null;
            string mensaje = string.Empty;

            // Parse incoming JSON string 'objeto' which comes from the client FormData
            Inventario inv = new Inventario();
            try
            {
                if (!string.IsNullOrEmpty(objeto))
                {
                    using (var doc = JsonDocument.Parse(objeto))
                    {
                        var root = doc.RootElement;

                        // Helper to read int that might be number or string
                        int ReadInt(JsonElement el, string propName)
                        {
                            if (!el.TryGetProperty(propName, out var p)) return 0;
                            try
                            {
                                if (p.ValueKind == JsonValueKind.Number && p.TryGetInt32(out var v)) return v;
                                if (p.ValueKind == JsonValueKind.String && int.TryParse(p.GetString(), out var v2)) return v2;
                            }
                            catch { }
                            return 0;
                        }

                        string? ReadString(JsonElement el, string propName)
                        {
                            if (!el.TryGetProperty(propName, out var p)) return null;
                            try { return p.ValueKind == JsonValueKind.String ? p.GetString() : p.ToString(); } catch { return null; }
                        }

                        // read basic fields (note client uses camelCase names)
                        inv.InventarioID = ReadInt(root, "inventarioID");
                        inv.Nombre = ReadString(root, "nombre") ?? string.Empty;
                        inv.Descripcion = ReadString(root, "descripcion") ?? string.Empty;

                        // tipoID/proveedorID/categoriaID may be objects ({ tipoID: 1, descripcion: '...' }) or plain numbers
                        if (root.TryGetProperty("tipoID", out var tipoEl))
                        {
                            if (tipoEl.ValueKind == JsonValueKind.Object && tipoEl.TryGetProperty("tipoID", out var tval))
                            {
                                inv.TipoID = tval.ValueKind == JsonValueKind.Number ? tval.GetInt32() : (int.TryParse(tval.GetString(), out var ti) ? ti : 0);
                            }
                            else if (tipoEl.ValueKind == JsonValueKind.Number) inv.TipoID = tipoEl.GetInt32();
                            else if (tipoEl.ValueKind == JsonValueKind.String && int.TryParse(tipoEl.GetString(), out var t2)) inv.TipoID = t2;
                        }

                        if (root.TryGetProperty("proveedorID", out var provEl))
                        {
                            if (provEl.ValueKind == JsonValueKind.Object && provEl.TryGetProperty("proveedorID", out var pval))
                            {
                                inv.ProveedorID = pval.ValueKind == JsonValueKind.Number ? pval.GetInt32() : (int.TryParse(pval.GetString(), out var pi) ? pi : 0);
                            }
                            else if (provEl.ValueKind == JsonValueKind.Number) inv.ProveedorID = provEl.GetInt32();
                            else if (provEl.ValueKind == JsonValueKind.String && int.TryParse(provEl.GetString(), out var p2)) inv.ProveedorID = p2;
                        }

                        if (root.TryGetProperty("categoriaID", out var catEl))
                        {
                            if (catEl.ValueKind == JsonValueKind.Object && catEl.TryGetProperty("categoriaID", out var cval))
                            {
                                inv.CategoriaID = cval.ValueKind == JsonValueKind.Number ? cval.GetInt32() : (int.TryParse(cval.GetString(), out var ci) ? ci : 0);
                            }
                            else if (catEl.ValueKind == JsonValueKind.Number) inv.CategoriaID = catEl.GetInt32();
                            else if (catEl.ValueKind == JsonValueKind.String && int.TryParse(catEl.GetString(), out var c2)) inv.CategoriaID = c2;
                        }

                        // precioUnitario may be string or number
                        if (root.TryGetProperty("precioUnitario", out var precioEl))
                        {
                            try
                            {
                                if (precioEl.ValueKind == JsonValueKind.Number && precioEl.TryGetDecimal(out var pd)) inv.PrecioUnitario = pd;
                                else if (precioEl.ValueKind == JsonValueKind.String)
                                {
                                    var precioStr = precioEl.GetString();
                                    if (!string.IsNullOrEmpty(precioStr))
                                    {
                                        var normalized = precioStr.Trim();
                                        if (normalized.Contains(",") && normalized.Contains(".")) normalized = normalized.Replace(".", "").Replace(",", ".");
                                        else if (normalized.Contains(",")) normalized = normalized.Replace(",", ".");
                                        if (decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed)) inv.PrecioUnitario = parsed;
                                    }
                                }
                            }
                            catch { }
                        }

                        // stock
                        if (root.TryGetProperty("stock", out var stockEl))
                        {
                            if (stockEl.ValueKind == JsonValueKind.Number && stockEl.TryGetInt32(out var s)) inv.Stock = s;
                            else if (stockEl.ValueKind == JsonValueKind.String && int.TryParse(stockEl.GetString(), out var s2)) inv.Stock = s2;
                        }

                        // fechaCaducidad (optional) - expect format yyyy-MM-dd or ISO
                        if (root.TryGetProperty("fechaCaducidad", out var fechaEl) && fechaEl.ValueKind == JsonValueKind.String)
                        {
                            var fstr = fechaEl.GetString();
                            if (!string.IsNullOrEmpty(fstr) && DateTime.TryParse(fstr, out var fdt)) inv.FechaCaducidad = fdt;
                        }

                        // urlImagen (optional)
                        if (root.TryGetProperty("urlImagen", out var urlEl) && urlEl.ValueKind == JsonValueKind.String)
                        {
                            inv.rutaimagen = urlEl.GetString() ?? inv.rutaimagen;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If parsing fails, return error
                return Json(new { resultado = (object?)null, mensaje = "Error al procesar datos del producto: " + ex.Message });
            }

            // Call business layer
            if (inv.InventarioID == 0)
            {
                resultado = new CN_Inventario().AgregarInventario(inv, out mensaje);
            }
            else
            {
                resultado = new CN_Inventario().EditarInventario(inv, out mensaje);
            }

            // Handle uploaded image file if present
            try
            {
                bool exitoGuardarImagen = false;

                int idInventario = inv.InventarioID;
                if (idInventario == 0 && resultado != null && int.TryParse(resultado.ToString(), out int idRes))
                {
                    idInventario = idRes;
                    inv.InventarioID = idInventario;
                }

                if (Imagen != null && Imagen.Length > 0 && idInventario > 0)
                {
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    var nombreUnico = Guid.NewGuid().ToString() + Path.GetExtension(Imagen.FileName);
                    var rutaRelativa = Path.Combine("/imagenes", nombreUnico).Replace("\\", "/");
                    var rutaFisica = Path.Combine(uploadPath, nombreUnico);

                    using (var stream = System.IO.File.Create(rutaFisica))
                    {
                        Imagen.CopyTo(stream);
                    }

                    inv.rutaimagen = rutaRelativa;
                    inv.nombreimagen = nombreUnico;

                    string msgImg = string.Empty;
                    exitoGuardarImagen = new CN_Inventario().GuardarImagen(idInventario, inv, out msgImg);
                    if (!exitoGuardarImagen && string.IsNullOrEmpty(mensaje)) mensaje = msgImg;
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(mensaje)) mensaje = ex.Message;
            }

            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Inventario? obj = new CN_Inventario().ListarInventario().Where(i => i.InventarioID == id).FirstOrDefault();

            if (obj == null)
            {
                return Json(new { conversion = false, data = string.Empty, extencion = string.Empty });
            }

            // Compose physical path under wwwroot if rutaimagen is a relative url like /imagenes/xxx.ext
            string fichero = string.Empty;
            if (!string.IsNullOrEmpty(obj.rutaimagen) && !string.IsNullOrEmpty(obj.nombreimagen))
            {
                // If rutaimagen looks like '/imagenes/xxx.ext', combine with wwwroot
                var possible = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", obj.nombreimagen);
                fichero = possible;
            }

            string textoBase64 = CN_recursos.ConvertirBase64(fichero, out conversion);
            var ext = string.Empty;
            if (!string.IsNullOrEmpty(obj.nombreimagen)) ext = Path.GetExtension(obj.nombreimagen);
            return Json(new { conversion = conversion, data = textoBase64, extencion = ext });

        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;
            resultado = new CN_Inventario().EliminarInventario(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        #endregion Producto

        //-----------------------------------------------------PROVEEDOR-----------------------------------------------------//
        public IActionResult Proveedor()
        {
            return View();
        }

        #region Proveedor

        [HttpGet]
        public JsonResult ListarProveedores()
        {
            List<Proveedor> oLista = new List<Proveedor>();
            oLista = new CN_Proveedor().ListarProveedor();
            Console.WriteLine($"Proveedores encontrados: {oLista.Count}");
            //retornar la lista en formato json
            return Json(new { data = oLista });
        }

        [HttpPost]
        public JsonResult GuardarProveedor([FromBody] Proveedor objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto == null)
            {
                return Json(new { resultado = false, mensaje = "Datos nulos" });
            }

            if (objeto.ProveedorID == 0)
            {
                resultado = new CN_Proveedor().AgregarProveedor(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Proveedor().EditarProveedor(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje });
        }
        [HttpPost]
        public JsonResult EliminarProveedor(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;
            resultado = new CN_Proveedor().EliminarProveedor(id, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        #endregion Proveedor
    }
}