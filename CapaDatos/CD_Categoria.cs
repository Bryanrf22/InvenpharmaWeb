using CapaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Categoria
    {
        public List<Categoria> ListarCategoria()
        {
            List<Categoria> lista = new List<Categoria>();
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("ListarCategoria", conexion.AbrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Categoria()
                        {
                            CategoriaID = Convert.ToInt32(dr["CategoriaID"]),
                            Nombre = dr["Nombre"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Categoria>();
            }
            finally
            {
                conexion.CerrarConexion();
            }

            return lista;
        }

        public int AgregarCategoria(Categoria obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("AgregarCategoria", conexion.AbrirConexion());

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

        public bool EditarCategoria(Categoria obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("EditarCategoria", conexion.AbrirConexion());
                cmd.Parameters.AddWithValue("@CategoriaID", obj.CategoriaID);
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

        public bool EliminarCategoria(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();

            try
            {
                using (SqlCommand cmd = new SqlCommand("EliminarCategoria", conexion.AbrirConexion()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CategoriaID", id);

                    // Parámetros de salida
                    SqlParameter parRes = new SqlParameter("@Resultado", SqlDbType.Bit);
                    parRes.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parRes);

                    SqlParameter parMsg = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500);
                    parMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parMsg);

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
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