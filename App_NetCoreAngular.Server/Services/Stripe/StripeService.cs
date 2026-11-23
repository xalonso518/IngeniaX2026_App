using System.Net;
using App_NetCoreAngular.Server.Models.Pago;
using App_NetCoreAngular.Server.Models.Shared;
using Stripe;
using Stripe.Checkout;
using static App_NetCoreAngular.Server.Enums.EnumSystem;

namespace App_NetCoreAngular.SendGrid
{

    public class StripeService
    {
        private readonly StripeClient _stripe;
        private const string DOMAIN = "https://localhost:4200";
        
        public StripeService(StripeClient stripe)
        {
            _stripe = stripe;
        }

        public async Task<ResponseApi<string>> Checkout(PagoForm form)
        {
            ResponseApi<string> res = new ResponseApi<string>() { Success = false };

            // Crear opciones de pago
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Precio id para el producto
                    Price = "price_1UJcgoLCBPKairBMJ9tsmmpA",
                    Quantity = form.Cantidad,
                  },
                },
                Mode = "payment",
                SuccessUrl = DOMAIN + "/pago/success",
                IntegrationIdentifier = "hosted_web_0001",
            };

            // Generar session para front
            Session session = await _stripe.V1.Checkout.Sessions.CreateAsync(options);
            res.Payload = session.Url;
            res.Success = true;

            return res;
        }
 
    }
}
