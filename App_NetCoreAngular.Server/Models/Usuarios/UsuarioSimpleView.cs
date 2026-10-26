using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Models.Usuarios
{
    public class UsuarioSimpleView
    {
        public long UsuarioId { get; set; }
        public required string Guid { get; set; }
        public required string Username { get; set; }
        public required string Nombre { get; set; }
        public string? ImagenPerfil { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Puesto { get; set; }
        public RolUsuario Rol { get; set; }
        public DateTime DateCreated { get; set; }
        public Estado Estado { get; set; }
    }
    public class UsuarioRelacionSimpleView
    {
        public long UsuarioId { get; set; }
        public required string Username { get; set; }
        public required string Nombre { get; set; }
        public RolUsuario Rol { get; set; }
        public Estado Estado { get; set; }
    }
}
