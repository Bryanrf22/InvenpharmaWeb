using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objCapaDatos = new CD_Reporte();

        public List<HistorialVentas> HistorialVentas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            return objCapaDatos.HistorialVentas(fechaInicio, fechaFin, idTransaccion);
        }

        public DashBoard Reporte()
        {
            return objCapaDatos.Reporte();
        }
    }
}