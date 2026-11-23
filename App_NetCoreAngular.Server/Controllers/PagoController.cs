using App_NetCoreAngular.SendGrid;
using App_NetCoreAngular.Server.Models.Pago;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace App_NetCoreAngular.Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PagoController : ControllerBase
    {
        private readonly StripeService _stripe;

        public PagoController(StripeService stripe)
        {
            _stripe = stripe;
        }

        ///<summary>
        ///Agrega producto
        ///</summary>
        [HttpPost("checkout")]
        //[Authorize(Policy = "UserActive")]
        public async Task<IActionResult> Agregar([FromBody] PagoForm form)
        {
            // Mandamos llamar servicio creado para stripe
            var res = await _stripe.Checkout(form);
            return Ok(res);
        }
    }
}

// Ctrl + I