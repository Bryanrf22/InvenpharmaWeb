using CapaEntidad;
using CapaNegocio;
using CapaPresentacionAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using ClosedXML.Excel;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Usuarios()
        {
            return View();
        }

        #region CRUD Usuarios

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuarios().ListarUsuarios();
            Console.WriteLine($"Usuarios encontrados: {oLista.Count}");

            //retornar la lista en formato json
            return Json(new { data = oLista });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public JsonResult GuardarUsuario([FromBody] Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.UsuarioID == 0)
            {
                resultado = new CN_Usuarios().RegistrarUsuario(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().EditarUsuario(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        public class IdRequest
        {
            public int id { get; set; }
        }

        [HttpPost]
        public JsonResult EliminarUsuario([FromBody] IdRequest request)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Usuarios().EliminarUsuario(request.id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        #endregion CRUD Usuarios

        #region Reportes

        [HttpGet]
        public JsonResult ObtenerReporte()
        {
            DashBoard objeto = new CN_Reporte().Reporte();
            return Json(new { resultado = objeto });
        }

        [HttpPost]
        public JsonResult HistorialVentas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<HistorialVentas> oLista = new List<HistorialVentas>();
            oLista = new CN_Reporte().HistorialVentas(fechaInicio, fechaFin, idTransaccion);
            return Json(new { data = oLista });
        }

        [HttpPost]
        public FileResult ExportarVentas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<HistorialVentas> oLista = new List<HistorialVentas>();
            oLista = new CN_Reporte().HistorialVentas(fechaInicio, fechaFin, idTransaccion);

            DataTable dt = new DataTable();

            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("ID Transacción", typeof(string));

            foreach (HistorialVentas hv in oLista)
            {
                dt.Rows.Add(new object[] {
                    hv.FechaVenta,
                    hv.Usuario,
                    hv.Producto,
                    hv.Cantidad,
                    hv.Precio,
                    hv.Total,
                    hv.TransaccionID
                });
            }
            dt.TableName = "HistorialVentas";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "HistorialVentas" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
                }
            }


        }


        #endregion Reportes
    }
}