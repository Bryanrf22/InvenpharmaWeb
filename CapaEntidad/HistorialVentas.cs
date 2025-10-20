using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CapaEntidad
{
    public class HistorialVentas
    {
        public string FechaVenta { get; set; }
        public string Usuario { get; set; }
        public string Producto { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string TransaccionID { get; set; }
    }
}