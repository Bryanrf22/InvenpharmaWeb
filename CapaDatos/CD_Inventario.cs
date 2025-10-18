using CapaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Inventario
    {
        public List<Inventario> ListarInventario()
        {
            List<Inventario> lista = new List<Inventario>();
            Conexion conexion = new Conexion();

            try
            {
                using (SqlCommand cmd = new SqlCommand("ListarInventario", conexion.AbrirConexion()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Inventario()
                            {
                                InventarioID = Convert.ToInt32(dr["InventarioID"]),
                                Nombre = Convert.ToString(dr["Nombre"]),
                                TipoID = Convert.ToInt32(dr["TipoID"]),
                                NomTipo = Convert.ToString(dr["NomTipo"]),
                                ProveedorID = Convert.ToInt32(dr["ProveedorID"]),
                                NomProv = Convert.ToString(dr["NomProv"]),
                                CategoriaID = Convert.ToInt32(dr["CategoriaID"]),
                                NomCat = Convert.ToString(dr["NomCat"]),
                                Descripcion = Convert.ToString(dr["Descripcion"]),
                                PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                                FechaCaducidad = dr["FechaCaducidad"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaCaducidad"]),
                                rutaimagen = Convert.ToString(dr["rutaimagen"]),
                                nombreimagen = Convert.ToString(dr["nombreimagen"]),
                                Stock = Convert.ToInt32(dr["Stock"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar o depurar error
                lista = new List<Inventario>();
                // Registrar mensaje de la excepción para diagnóstico
                Console.WriteLine("Error ListarInventario: " + ex.Message);
            }
            finally
            {
                conexion.CerrarConexion();
            }

            return lista;
        }

        public int AgregarProducto(Inventario obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand("AgregarProducto", conexion.AbrirConexion()))
                {
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@TipoID", obj.TipoID);
                    cmd.Parameters.AddWithValue("@ProveedorID", obj.ProveedorID);
                    cmd.Parameters.AddWithValue("@CategoriaID", obj.CategoriaID);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", obj.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@FechaCaducidad", (object?)obj.FechaCaducidad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Stock", obj.Stock);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value?.ToString() ?? string.Empty;
                }
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

        public bool EditarProducto(Inventario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EditarProducto", conexion.AbrirConexion()))
                {
                    cmd.Parameters.AddWithValue("@InventarioID", obj.InventarioID);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@TipoID", obj.TipoID);
                    cmd.Parameters.AddWithValue("@ProveedorID", obj.ProveedorID);
                    cmd.Parameters.AddWithValue("@CategoriaID", obj.CategoriaID);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", obj.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@FechaCaducidad", (object?)obj.FechaCaducidad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Stock", obj.Stock);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToInt32(cmd.Parameters["Resultado"].Value) == 1;
                    Mensaje = cmd.Parameters["Mensaje"].Value?.ToString() ?? string.Empty;
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

        public bool EliminarProducto(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion conexion = new Conexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EliminarProducto", conexion.AbrirConexion()))
                {
                    cmd.Parameters.AddWithValue("@InventarioID", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToInt32(cmd.Parameters["Resultado"].Value) == 1;
                    Mensaje = cmd.Parameters["Mensaje"].Value?.ToString() ?? string.Empty;
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

        public bool GuardarDatosImagen(int id, Inventario Inventario, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new Conexion().AbrirConexion())
                using (SqlCommand cmd = new SqlCommand("GuardarDatosImagen", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@InventarioID", Inventario.InventarioID);
                    cmd.Parameters.AddWithValue("@rutaimagen", Inventario.rutaimagen ?? string.Empty);
                    cmd.Parameters.AddWithValue("@nombreimagen", Inventario.nombreimagen ?? string.Empty);

                    // Ejecutar y comprobar filas afectadas (el stored procedure presentado realiza un UPDATE)
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        resultado = true;
                        Mensaje = string.Empty;
                    }
                    else
                    {
                        resultado = false;
                        Mensaje = "No se encontró el registro o no se pudo actualizar la imagen.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            finally
            {
                new Conexion().CerrarConexion();
            }
            return resultado;
        }
    }
}