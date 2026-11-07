using App_NetCoreAngular.Server.Utility.Error;
using System.ComponentModel.DataAnnotations;

namespace App_NetCoreAngular.Server.Models.Productos
{
    public class ProductoFormEditar
    {
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required long ProductoId { get; set; }
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(250, ErrorMessage = ErrorMessages.Max250)]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(50, ErrorMessage = ErrorMessages.Max50)]
        public required string Codigo { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(500, ErrorMessage = ErrorMessages.Max500)]
        public required string Descripcion { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required decimal Precio { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required decimal Existencia { get; set; }
    }
}
