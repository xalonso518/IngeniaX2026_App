using App_NetCoreAngular.Server.Utility.Error;
using System.ComponentModel.DataAnnotations;

namespace App_NetCoreAngular.Server.Models.Pago
{
    public class PagoForm
    {

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(50, ErrorMessage = ErrorMessages.Max50)]
        public required string Codigo { get; set; }
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MaxLength(50, ErrorMessage = ErrorMessages.Max50)]
        public required string PriceId { get; set; }
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required int Cantidad { get; set; }
    }
}
