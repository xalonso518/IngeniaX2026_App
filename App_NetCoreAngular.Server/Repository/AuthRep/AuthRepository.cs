using App_NetCoreAngular.Server.EntityFramework;
using App_NetCoreAngular.Server.Models.Auth;
using App_NetCoreAngular.Server.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Repository.AuthRep
{
    public class AuthRepository: IAuthRepository
    {
        private readonly AppDataContext _context;
        public AuthRepository(
            AppDataContext context)
        {
            _context = context;
        }
        public async Task<ResponseApi<AuthUsuarioView>> GetUsuarioPassword(LoginForm form)
        {
            var res = new ResponseApi<AuthUsuarioView>() { Success = false };

            try
            {
               var rUser = await _context.Usuarios
                   .Where(r => r.Username == form.Email && r.Password == form.Password && r.Estado == Estado.Activo)
                   .Select(r => new AuthUsuarioView() { 
                       UsuarioId = r.UsuarioId,
                       Guid = r.Guid,
                       Nombre = r.Nombre,
                       ImagenPerfil = r.ImagenPerfil??"",
                       Rol = r.Rol,
                       Estado = r.Estado,
                       Username = r.Username
                   }).FirstOrDefaultAsync();

                   if (rUser != null)
                   {
                       res.Success = true;
                       res.Payload = rUser;
                   }
                   else
                   {
                       res.Success = false;
                       res.Error = "No se encontró usuario.";
                   }                
                }
                catch (Exception)
                {
                   res.Error = "Error en la base de datos.";
                }
            return res;
        }
        public async Task<ResponseApi<string>> ValidateUsuario(ResetForm form)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseApi<HttpStatusCode>> EnviarCorreoReset(string correo, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseApi<string>> UpdatePassword(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
