using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Models.Auth
{
    public class AuthUsuarioView
    {
        public long UsuarioId { get; set; }
        public required string Guid { get; set; }
        public required string Username { get; set; }
        public required string Nombre { get; set; }
        public required string ImagenPerfil { get; set; }
        public RolUsuario Rol { get; set; }
        public bool LoginFirstTime { get; set; }
        public Estado Estado { get; set; }
    }
}
