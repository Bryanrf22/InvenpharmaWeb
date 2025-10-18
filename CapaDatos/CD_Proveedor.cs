using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Proveedor
    {
        public List<Proveedor> ListarProveedor()
        {
            List<Proveedor> lista = new List<Proveedor>();
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("ListarProveedor", conexion.AbrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Proveedor()
                        {
                            ProveedorID = Convert.ToInt32(dr["ProveedorID"]),
                            Nombre = dr["Nombre"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Direccion = dr["Direccion"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Proveedor>();
            }
            finally
            {
                conexion.CerrarConexion();
            }
            return lista;
        }

        public int AgregarProveedor(Proveedor obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("AgregarProveedor", conexion.AbrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                cmd.Parameters.AddWithValue("@Telefono", obj.Telefono);
                cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                cmd.Parameters.AddWithValue("@Direccion", obj.Direccion);
                cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
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

        public bool EditarProveedor(Proveedor obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            using (var conexion = new Conexion().AbrirConexion())
            using (SqlCommand cmd = new SqlCommand("EditarProveedor", conexion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProveedorID", obj.ProveedorID);
                cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                cmd.Parameters.AddWithValue("@Telefono", obj.Telefono);
                cmd.Parameters.AddWithValue("@Direccion", obj.Direccion);

                cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                int res = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();

                resultado = (res == 1);
            }
            return resultado;
        }

        public bool EliminarProveedor(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                SqlCommand cmd = new SqlCommand("EliminarProveedor", conexion.AbrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProveedorID", id);
                cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
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