using System.ComponentModel.DataAnnotations;
using App_NetCoreAngular.Server.Utility.Error;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Models.Usuarios
{

    public class UsuarioFormEditar
    {
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required long UsuarioId { get; set; }


        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(512, ErrorMessage = ErrorMessages.Max500)]
        public required string Guid { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(250, ErrorMessage = ErrorMessages.Max250)]
        public required string Nombre { get; set; }

        [MaxLength(50, ErrorMessage = ErrorMessages.Max50)]
        public string? Email { get; set; }

        [MaxLength(250, ErrorMessage = ErrorMessages.Max250)]
        public string? Telefono { get; set; }

        [MaxLength(250, ErrorMessage = ErrorMessages.Max250)]
        public string? Puesto { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public RolUsuario Rol { get; set; }
    }
}
