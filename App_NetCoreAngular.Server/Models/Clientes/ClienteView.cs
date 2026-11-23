using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Models.Clientes
{
    public class ClienteView
    {
        public long ClienteId { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public Estado Estado { get; set; }
    }
}
