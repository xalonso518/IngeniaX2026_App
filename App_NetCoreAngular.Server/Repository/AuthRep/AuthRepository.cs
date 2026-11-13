using App_NetCoreAngular.SendGrid;
using App_NetCoreAngular.Server.EntityFramework;
using App_NetCoreAngular.Server.Models.Auth;
using App_NetCoreAngular.Server.Models.Mail;
using App_NetCoreAngular.Server.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Repository.AuthRep
{
    public class AuthRepository: IAuthRepository
    {
        private readonly AppDataContext _context;
        private readonly MailSendGrid _mailSend;
        public AuthRepository(
            AppDataContext context,
            MailSendGrid mailSend)
        {
            _context = context;
            _mailSend = mailSend;
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
            var res = new ResponseApi<string>() { Success = false };
            try
            {
                var rUser = await _context.Usuarios
                    .Where(r => r.Username == form.Email && r.Estado == Estado.Activo)
                    .Select(r => r.Username).FirstOrDefaultAsync();

                if (rUser != null)
                {
                    res.Success = true;
                    res.Payload = rUser;
                }
                else
                {
                    res.Success = false;
                    res.Error = "";
                }
            }
            catch (Exception)
            {
                res.Error = "";
            }
            return res;
        }

        public async Task<ResponseApi<HttpStatusCode>> EnviarCorreoReset(string correo, string password)
        {
            // Crea objeto de correo
            var mailBody = new Mail
            {
                Subject = "Contraseña restablecida",
                To = correo,
                Substitution = new Dictionary<string, string>() { { "password", password }, { "user", correo } }
            };

            //Envia correo de reset
            var resultMail = await _mailSend.SendTemplateEmail(mailBody, "d-43da04d10f77498b9e553b838269e148");
            return resultMail;
        }

        public async Task<ResponseApi<string>> UpdatePassword(string userName, string password)
        {
            var res = new ResponseApi<string>() { Success = false };

            try
            {
                var response = await _context.Usuarios
                    .Where(e => e.Username == userName)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Password, password));

                if (response != 0)
                {
                    res.Success = true;
                    res.Payload = userName;
                }
                else
                {
                    res.Success = false;
                    res.Payload = "";
                }
            }
            catch (Exception)
            {
                res.Error = "";
            }
            return res;
        }
    }
}
