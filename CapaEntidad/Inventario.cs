namespace CapaEntidad
{
    public class Inventario
    {
        public int InventarioID { get; set; }
        public string Nombre { get; set; }
        public int TipoID { get; set; }
        public int ProveedorID { get; set; }
        public int CategoriaID { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public string rutaimagen { get; set; }
        public string nombreimagen { get; set; }
        public int Stock { get; set; }

        // Campos adicionales que vienen del JOIN
        public string NomTipo { get; set; }

        public string NomProv { get; set; }
        public string NomCat { get; set; }

        //campos extras
        public string Base64 { get; set; }
        public string Extension { get; set; }
    }
}