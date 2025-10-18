using CapaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public DashBoard dashBoards = new DashBoard();

        public DashBoard Reporte()
        {
            DashBoard dashBoard = new DashBoard();
            Conexion conexion = new Conexion();

            try
            {
                SqlCommand cmd = new SqlCommand("ReporteDashboard", conexion.AbrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    dashBoard.TotalCliente = Convert.ToInt32(dr["TotalCliente"]);
                    dashBoard.TotalVenta = Convert.ToInt32(dr["TotalVenta"]);
                    dashBoard.TotalProducto = Convert.ToInt32(dr["TotalProducto"]);
                    dashBoard.TotalProveedor = Convert.ToInt32(dr["TotalProveedor"]);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                dashBoard = new DashBoard(); // valores por defecto
            }
            finally
            {
                conexion.CerrarConexion();
            }

            return dashBoard;
        }
    }
}