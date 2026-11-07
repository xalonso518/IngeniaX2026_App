namespace App_NetCoreAngular.Server.Models.Productos
{
    public class ProductoView: ProductoSimpleView
    {
        public required string Descripcion { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
