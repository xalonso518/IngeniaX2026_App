using App_NetCoreAngular.Server.Utility.Error;
using System.ComponentModel.DataAnnotations;

namespace App_NetCoreAngular.Server.Models.Auth
{
    public class LoginForm
    {
        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        public required string Email { get; set; }

        [Required(ErrorMessage = ErrorMessages.Obligatorio)]
        [MinLength(5, ErrorMessage = ErrorMessages.Min5)]
        public required string Password { get; set; }

    }
}
