using System.ComponentModel.DataAnnotations;
using App_NetCoreAngular.Server.Utility.Error;

namespace App_NetCoreAngular.Server.Models.Productos
{
    public class ProductoFormImg
    {
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public IFormFile? Archivo { get; set; }
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required string Nombre { get; set; }
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required string ProductoId { get; set; }
    }
}
