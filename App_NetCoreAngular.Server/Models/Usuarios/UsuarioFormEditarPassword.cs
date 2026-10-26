using System.ComponentModel.DataAnnotations;
using App_NetCoreAngular.Server.Utility.Error;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Models.Usuarios
{
    public class UsuarioFormEditarPassword
    {
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required long UsuarioId { get; set; }


        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(512, ErrorMessage = ErrorMessages.Max500)]
        public required string Guid { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(50, ErrorMessage = ErrorMessages.Max50)]
        public required string Password { get; set; }
    }
}
