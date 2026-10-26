using System.Net;
using App_NetCoreAngular.Server.Models.Shared;
using App_NetCoreAngular.Server.Models.Usuarios;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Repository.UsuariorRep
{
    public interface IUsuarioRepository
    {
        Usuario Crear(UsuarioForm usuario);
        Task<ResponseApi<long>> Agregar(Usuario usuario);
        Task<ResponseApi<string>> Editar(UsuarioFormEditar form);
        Task<ResponseApi<List<UsuarioSimpleView>>> Buscar(string page, string sortBy, string rol, string? word = "");
        Task<ResponseApi<UsuarioView>> BuscarPorGuid(string guid);
        Task<ResponseApi<string>> EditarEstado(long usuarioId, Estado estado);
        Task<ResponseApi<string>> EditarPassword(long usuarioId, string password);
        Task<ResponseApi<string>> EditarImagenPerfil(long usuarioId, string img);
        Task<ResponseApi<string>> EliminarUsuario(string guid);
        Task<ResponseApi<bool>> ValidarExistencia(string username);
    }
}