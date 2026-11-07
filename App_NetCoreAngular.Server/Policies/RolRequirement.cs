using Microsoft.AspNetCore.Authorization;

namespace App_NetCoreAngular.Server.Policies
{
    public class RolRequirement : IAuthorizationRequirement
    {
        public RolRequirement(string rol)
        {
            Rol = rol;
        }

        public string Rol { get; set; }
    }
}
