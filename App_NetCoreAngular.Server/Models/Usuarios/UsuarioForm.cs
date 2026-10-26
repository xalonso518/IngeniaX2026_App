using System.ComponentModel.DataAnnotations;
using App_NetCoreAngular.Server.Utility.Error;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Models.Usuarios
{
    public class UsuarioForm
    {
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(250, ErrorMessage = ErrorMessages.Max250)]
        public required string Username { get; set; }


        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MinLength(5, ErrorMessage = ErrorMessages.Min5)]
        [MaxLength(25, ErrorMessage = ErrorMessages.Max25)]
        public required string Password { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(250, ErrorMessage = ErrorMessages.Max250)]
        public required string Nombre { get; set; }

        [MaxLength(50, ErrorMessage = ErrorMessages.Max50)]
        public string? Email { get; set; }

        [MaxLength(250, ErrorMessage = ErrorMessages.Max250)]
        public string? Telefono { get; set; }

        [MaxLength(250, ErrorMessage = ErrorMessages.Max250)]
        public  string? Puesto { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public RolUsuario RolUsuario { get; set; }
    }
}
