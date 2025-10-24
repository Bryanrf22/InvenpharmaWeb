using CapaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("ListarUsuarios", conexion.AbrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Usuario()
                        {
                            UsuarioID = Convert.ToInt32(dr["UsuarioID"]),
                            Correo = dr["Correo"].ToString(),
                            clave = dr["clave"].ToString(),
                            administrador = Convert.ToBoolean(dr["administrador"]),
                            activo = Convert.ToBoolean(dr["activo"]),
                            reestablecer = Convert.ToBoolean(dr["reestablecer"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Usuario>();
            }
            finally
            {
                conexion.CerrarConexion();
            }

            return lista;
        }

        public int RegistrarUsuario(Usuario obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("AgregarUsuario", conexion.AbrirConexion());

                cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                cmd.Parameters.AddWithValue("@Clave", obj.clave);
                cmd.Parameters.AddWithValue("@Admin", obj.administrador);
                cmd.Parameters.AddWithValue("@Activo", obj.activo);
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

        public bool EditarUsuario(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("EditarUsuario", conexion.AbrirConexion());
                cmd.Parameters.AddWithValue("@UsuarioID", obj.UsuarioID);
                cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                cmd.Parameters.AddWithValue("@Admin", obj.administrador);
                cmd.Parameters.AddWithValue("@Activo", obj.activo);
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

        public bool EliminarUsuario(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();

            try
            {
                using (SqlCommand cmd = new SqlCommand("EliminarUsuario", conexion.AbrirConexion()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UsuarioID", id);

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

        public bool CambiarClave(int idusuario, string nuevaClave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("CambiarClaveUsuario", conexion.AbrirConexion());
                cmd.Parameters.AddWithValue("@UsuarioID", idusuario);
                cmd.Parameters.AddWithValue("@NuevaClave", nuevaClave);
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

        public bool ReestablecerClave(int idusuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("ReestablecerClaveUsuario", conexion.AbrirConexion());
                cmd.Parameters.AddWithValue("@UsuarioID", idusuario);
                cmd.Parameters.AddWithValue("@Clave", clave);
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