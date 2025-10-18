using CapaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Tipo
    {
        public List<Tipo> Listartipo()
        {
            List<Tipo> lista = new List<Tipo>();
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("ListarTipo", conexion.AbrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Tipo()
                        {
                            TipoID = Convert.ToInt32(dr["TipoID"]),
                            Nombre = dr["Nombre"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Tipo>();
            }
            finally
            {
                conexion.CerrarConexion();
            }

            return lista;
        }

        public int AgregarTipo(Tipo obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("AgregarTipo", conexion.AbrirConexion());

                cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();

                idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            finally
            {
                conexion.CerrarConexion();
            }
            return idautogenerado;
        }

        public bool EditarTipo(Tipo obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("EditarTipo", conexion.AbrirConexion());
                cmd.Parameters.AddWithValue("@TipoID", obj.TipoID);
                cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            finally
            {
                conexion.CerrarConexion();
            }
            return resultado;
        }

        public bool EliminarTipo(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("EliminarTipo", conexion.AbrirConexion());
                cmd.Parameters.AddWithValue("@TipoID", id);
                cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            finally
            {
                conexion.CerrarConexion();
            }
            return resultado;
        }
    }
}