using App_NetCoreAngular.Server.Models.Clientes;
using App_NetCoreAngular.Server.Models.Shared;

namespace App_NetCoreAngular.Server.Repository.ClienteRep
{
    public interface IClienteRepository
    {
        Task<ResponseApi<long>> Agregar(ClienteForm form);
        Task<ResponseApi<List<ClienteView>>> Buscar();
        Task<ResponseApi<ClienteView>> Obtener(long clienteId);
        Task<ResponseApi<string>> Editar(long clienteId, ClienteForm form);
        Task<ResponseApi<string>> Eliminar(long clienteId);
    }
}
