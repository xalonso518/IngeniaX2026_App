using App_NetCoreAngular.Server.EntityFramework;
using App_NetCoreAngular.Server.Models.Clientes;
using App_NetCoreAngular.Server.Models.Shared;
using Microsoft.EntityFrameworkCore;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Repository.ClienteRep
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDataContext _context;

        public ClienteRepository(AppDataContext context)
        {
            _context = context;
        }

        public async Task<ResponseApi<long>> Agregar(ClienteForm form)
        {
            var cliente = new Cliente
            {
                Nombre = form.Nombre,
                Descripcion = form.Descripcion,
                FechaAlta = DateTime.UtcNow,
                Estado = Estado.Activo
            };

            

            await _context.Clientes.AddAsync(cliente);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0
                ? new ResponseApi<long> { Success = true, Payload = cliente.ClienteId }
                : new ResponseApi<long> { Error = "No se pudo agregar el cliente." };
        }

        public async Task<ResponseApi<List<ClienteView>>> Buscar()
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .Where(cliente => cliente.Estado != Estado.Baja)
                .OrderBy(cliente => cliente.Nombre)
                .Select(cliente => new ClienteView
                {
                    ClienteId = cliente.ClienteId,
                    Nombre = cliente.Nombre,
                    Descripcion = cliente.Descripcion,
                    FechaAlta = cliente.FechaAlta,
                    Estado = cliente.Estado
                })
                .ToListAsync();

            return new ResponseApi<List<ClienteView>>
            {
                Success = true,
                Payload = clientes
            };
        }

        public async Task<ResponseApi<ClienteView>> Obtener(long clienteId)
        {
            var cliente = await _context.Clientes
                .AsNoTracking()
                .Where(cliente => cliente.ClienteId == clienteId && cliente.Estado != Estado.Baja)
                .Select(cliente => new ClienteView
                {
                    ClienteId = cliente.ClienteId,
                    Nombre = cliente.Nombre,
                    Descripcion = cliente.Descripcion,
                    FechaAlta = cliente.FechaAlta,
                    Estado = cliente.Estado
                })
                .FirstOrDefaultAsync();

            return cliente is not null
                ? new ResponseApi<ClienteView> { Success = true, Payload = cliente }
                : new ResponseApi<ClienteView> { Error = "No se encontró el cliente." };
        }

        public async Task<ResponseApi<string>> Editar(long clienteId, ClienteForm form)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(cliente =>
                    cliente.ClienteId == clienteId && cliente.Estado != Estado.Baja);

            if (cliente is null)
            {
                return new ResponseApi<string> { Error = "No se encontró el cliente." };
            }

            cliente.Nombre = form.Nombre;
            cliente.Descripcion = form.Descripcion;
            await _context.SaveChangesAsync();

            return new ResponseApi<string> { Success = true, Payload = clienteId.ToString() };
        }

        public async Task<ResponseApi<string>> Eliminar(long clienteId)
        {
            var rowsAffected = await _context.Clientes
                .Where(cliente => cliente.ClienteId == clienteId && cliente.Estado != Estado.Baja)
                .ExecuteUpdateAsync(setters => setters.SetProperty(cliente => cliente.Estado, Estado.Baja));

            return rowsAffected > 0
                ? new ResponseApi<string> { Success = true, Payload = clienteId.ToString() }
                : new ResponseApi<string> { Error = "No se encontró el cliente o ya está dado de baja." };
        }
    }
}
