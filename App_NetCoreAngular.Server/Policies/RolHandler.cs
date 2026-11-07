using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace App_NetCoreAngular.Server.Policies
{
    public class RolHandler : AuthorizationHandler<RolRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolRequirement requirement)
        {
            //Valida si tiene claim el token
            if (!context.User.HasClaim(c => c.Type == "rol")) return Task.CompletedTask;

            //Obtiene el claim del token
            var rol = context.User.Claims.Where(c => c.Type == "rol").First<Claim>().Value;

            //Valida si el rol es como el solicitado (Admin)
            if (rol == requirement.Rol) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
