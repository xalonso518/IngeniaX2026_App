using App_NetCoreAngular.Server.Utility.Error;
using System.ComponentModel.DataAnnotations;

namespace App_NetCoreAngular.Server.Models.Auth
{
    public class ResetForm
    {
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required string Email { get; set; }
    }
}
