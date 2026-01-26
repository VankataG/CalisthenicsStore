using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace CalisthenicsStore.Web.Controllers
{
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly StripeSettings stripeSettings;

        private readonly IOrderService orderService;


        public StripeController(IOptions<StripeSettings> stripeOptions, IOrderService orderService)
        {
            this.stripeSettings = stripeOptions.Value;
            this.orderService = orderService;
        }


        [HttpPost("stripe/webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, stripeSettings.WebhookSecret);

            } 
            catch
            {
                return BadRequest();
            }

            if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                if (session?.Metadata != null &&
                    session.Metadata.TryGetValue("orderId", out var orderIdStr) &&
                    Guid.TryParse(orderIdStr, out var orderId))
                {
                    await orderService.MarkOrderAsPaidAsync(orderId);
                }
            }

            return Ok();
        }
    }
}
