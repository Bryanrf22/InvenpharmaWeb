using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DetalleVenta
    {
        public int DetalleVentaID { get; set; }
        public int VentaID { get; set; }
        public int InventarioID { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}