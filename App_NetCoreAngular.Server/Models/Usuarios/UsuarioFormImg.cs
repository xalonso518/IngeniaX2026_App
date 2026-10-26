using System.ComponentModel.DataAnnotations;
using App_NetCoreAngular.Server.Utility.Error;

namespace App_NetCoreAngular.Server.Models.Usuarios
{
    public class UsuarioFormImg
    {
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public IFormFile? Archivo { get; set; }
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required string Nombre { get; set; }
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required string UsuarioId { get; set; }


        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(512, ErrorMessage = ErrorMessages.Max500)]
        public required string Guid { get; set; }

    }
}
