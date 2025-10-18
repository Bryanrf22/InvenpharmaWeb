using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int VentaID { get; set; }
        public int UsuarioID { get; set; }
        public int CantidadVendida { get; set; }
        public decimal MontoTotal { get; set; }
        public string TransaccionID { get; set; }
    }
}