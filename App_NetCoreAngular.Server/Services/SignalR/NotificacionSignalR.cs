using Microsoft.AspNetCore.SignalR;

namespace App_NetCoreAngular.Server.Services.SignalR
{

    public class NotificacionSignalR : IHostedService
    {
        private readonly IHubContext<MainHub> _hubContext;

        public NotificacionSignalR(IHubContext<MainHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("SignalR iniciado");
            return Task.CompletedTask;
        }

        // Enviar notificacion a userName
        public async Task<bool> SendAsync(string username, string mensaje)
        {
            try
            {
                await _hubContext.Clients.User(username).SendAsync("ReceiveNotificationUser", mensaje);
                //await _hubContext.Clients.User(username).SendAsync("AltaProducto", mensaje);
                //await _hubContext.Clients.User(username).SendAsync("AltaUsuario", mensaje);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        // Enviar notificacion a multiples username
        public async Task<bool> SendAsync(List<string> usuarios, string mensaje)
        {
            try
            {
                // Recorremos usuarios
                for (var i = 0; i <= usuarios.Count; i++)
                {
                    // Enviamos notificacion
                    await _hubContext.Clients.User(usuarios[i]).SendAsync("ReceiveNotificationUser", mensaje);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("SignalR cerrado");
            return Task.CompletedTask;
        }
    }
}
