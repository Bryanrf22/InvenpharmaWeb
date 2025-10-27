using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class Conexion
    {

        private static readonly string _defaultConn = "Data Source=LAPTOP-JP09F1KB\\SQLEXPRESS;Initial Catalog=BDFarmaciaWeb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private static readonly string _connString = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CONNECTION_STRING"))
            ? Environment.GetEnvironmentVariable("CONNECTION_STRING")!
            : _defaultConn;

        
        private SqlConnection? cn;

        public SqlConnection AbrirConexion()
        {
            try
            {
                if (cn == null)
                {
                    cn = new SqlConnection(_connString);
                }

                if (cn.State == System.Data.ConnectionState.Closed)
                {
                    cn.Open();
                }

                return cn;
            }
            catch (Exception ex)
            {
                try { System.Console.Error.WriteLine($"[Conexion.AbrirConexion] Error al abrir conexión: {ex.Message}"); } catch { }
                throw;
            }
        }

        public void CerrarConexion()
        {
            try
            {
                if (cn != null)
                {
                    if (cn.State == System.Data.ConnectionState.Open)
                    {
                        cn.Close();
                    }

                    cn.Dispose();
                    cn = null;
                }
            }
            catch (Exception ex)
            {
                try { System.Console.Error.WriteLine($"[Conexion.CerrarConexion] Error al cerrar conexión: {ex.Message}"); } catch { }
            }
        }
    }
}