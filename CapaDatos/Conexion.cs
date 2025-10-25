using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class Conexion
    {
        // Se permite sobreescribir la cadena de conexión mediante la variable de entorno CONNECTION_STRING
        private static readonly string _defaultConn = "Data Source=LAPTOP-JP09F1KB\\SQLEXPRESS;Initial Catalog=BDFarmaciaWeb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private static readonly string _connString = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CONNECTION_STRING"))
            ? Environment.GetEnvironmentVariable("CONNECTION_STRING")!
            : _defaultConn;

        public SqlConnection cn = new SqlConnection(_connString);

        public SqlConnection AbrirConexion()
        {
            if (cn.State == System.Data.ConnectionState.Closed)
            {
                cn.Open();
            }
            return cn;
        }

        public SqlConnection CerrarConexion()
        {
            if (cn.State == System.Data.ConnectionState.Open)
            {
                cn.Close();
            }
            return cn;
        }
    }
}