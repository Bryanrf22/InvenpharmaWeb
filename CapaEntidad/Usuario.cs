namespace CapaEntidad
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Correo { get; set; }
        public string clave { get; set; }
        public bool administrador { get; set; }
        public bool activo { get; set; }
    }
}