using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class Conexion
    {
        public SqlConnection cn = new SqlConnection("Data Source=LAPTOP-JP09F1KB\\SQLEXPRESS;Initial Catalog=BDFarmaciaWeb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

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