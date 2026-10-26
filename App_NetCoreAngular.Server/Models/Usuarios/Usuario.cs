using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Models.Usuarios
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Required]
        public long UsuarioId { get; set; }
        [Required]
        [StringLength(512)]
        public required string Guid { get; set; }
        [Required]
        [StringLength(100)]
        public required string Username { get; set; }
        [Required]
        [StringLength(256)]
        public required string Password { get; set; }
        [StringLength(250)]
        public required string Nombre { get; set; }
        [StringLength(250)]
        public string? ImagenPerfil { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        [StringLength(250)]
        public string? Telefono { get; set; }
        [Required]
        public RolUsuario Rol { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required]
        public Estado Estado { get; set; }
    }
}
