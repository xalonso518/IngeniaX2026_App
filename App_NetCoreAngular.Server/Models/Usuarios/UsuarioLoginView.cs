using static App_NetCoreAngular.Server.Enums.EnumDatabase;
namespace App_NetCoreAngular.Server.Models.Usuarios
{
    public class UsuarioLoginView
    {
        public long UsuarioId { get; set; }
        public string? Guid { get; set; }
        public required string Username { get; set; }
        public required string Nombre { get; set; }
        public string? ImagenPerfil { get; set; }
        public string? Container { get; set; }
        public RolUsuario Rol { get; set; }
        public bool LoginFirstTime { get; set; }
        public Estado Estado { get; set; }
    }
}
