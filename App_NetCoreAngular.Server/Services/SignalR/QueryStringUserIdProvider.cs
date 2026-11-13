using Microsoft.AspNetCore.SignalR;

namespace App_NetCoreAngular.Server.Services.SignalR
{

    public class QueryStringUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // Obtenemos valores del JWT claim, username = userId
            var username = connection.User?.FindFirst("user")?.Value;

            // Devolvemos username
            if (!string.IsNullOrEmpty(username))
            {
                return username;
            }

            //Console.WriteLine("Se conecrto user: " + connection.UserIdentifier);
            //Console.WriteLine("Se conecrto conn: " + connection.ConnectionId);
            return connection.GetHttpContext()?.Request.Query["userId"];
        }
    }
}
