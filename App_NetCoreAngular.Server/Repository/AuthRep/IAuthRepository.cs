using App_NetCoreAngular.Server.Models.Auth;
using App_NetCoreAngular.Server.Models.Shared;
using System.Net;

namespace App_NetCoreAngular.Server.Repository.AuthRep
{
    public interface IAuthRepository
    {
        Task<ResponseApi<AuthUsuarioView>> GetUsuarioPassword(LoginForm usuario);
        Task<ResponseApi<string>> UpdatePassword(string userName, string password);
        Task<ResponseApi<string>> ValidateUsuario(ResetForm form);
        Task<ResponseApi<HttpStatusCode>> EnviarCorreoReset(string correo, string password);
    }
}
