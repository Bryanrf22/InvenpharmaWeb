using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Formulario
    {
        public int FormularioID { get; set; }
        public int UsuarioID { get; set; }
        public string Correo { get; set; }
        public string Consulta { get; set; }
    }
}