using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace App_NetCoreAngular.Server.Policies
{
    public class StatusUserHandler : AuthorizationHandler<StatusUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StatusUserRequirement requirement)
        {
            //Valida si tiene claim el token
            if (!context.User.HasClaim(c => c.Type == "stat")) return Task.CompletedTask;

            //Obtiene el claim del token          
            var status = context.User.Claims.Where(c => c.Type == "stat").First<Claim>().Value;

            //Valida si el estatus es como el solicitado(Activo)
            if (status == requirement.Status) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
