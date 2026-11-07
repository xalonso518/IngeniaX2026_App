using Microsoft.AspNetCore.Authorization;

namespace App_NetCoreAngular.Server.Policies
{
    public class StatusUserRequirement : IAuthorizationRequirement
    {
        public StatusUserRequirement(string status)
        {
            Status = status;
        }

        public string Status { get; set; }
    }
}
