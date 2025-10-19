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
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    // Helper to try multiple possible column names safely
                    int ReadAny(SqlDataReader reader, params string[] names)
                    {
                        foreach (var n in names)
                        {
                            try
                            {
                                int ord = reader.GetOrdinal(n);
                                if (!reader.IsDBNull(ord)) return Convert.ToInt32(reader.GetValue(ord));
                                return 0;
                            }
                            catch { /* column not present - try next */ }
                        }
                        return 0;
                    }

                    if (dr.Read())
                    {
                        dashBoard.TotalCliente = ReadAny(dr, "TotalCliente");
                        dashBoard.TotalVenta = ReadAny(dr, "TotalVenta");
                        // Stored proc previously returned TotalProductos (plural) in some versions
                        dashBoard.TotalProducto = ReadAny(dr, "TotalProducto", "TotalProductos");
                        dashBoard.TotalProveedor = ReadAny(dr, "TotalProveedor");
                    }
                }
            }
            catch (Exception ex)
            {
                // If anything fails, return a default object but log the exception for diagnosis
                Console.WriteLine("Error en ReporteDashboard: " + ex.Message);
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