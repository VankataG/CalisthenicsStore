using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Stripe;
using Stripe.Checkout;

using CalisthenicsStore.Data.Models.ReCaptcha;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Order;
using CalisthenicsStore.ViewModels.Payment;
using CalisthenicsStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using static CalisthenicsStore.Common.Constants.Notifications;

namespace CalisthenicsStore.Web.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;

        private readonly IReCaptchaServ captchaService;

        private readonly IOptions<GoogleReCaptchaSettings> captchaSett;

        public OrderController(IOrderService orderService, IReCaptchaServ captchaService, IOptions<GoogleReCaptchaSettings> captchaSett)
        {
            this.orderService = orderService;
            this.captchaService = captchaService;
            this.captchaSett = captchaSett;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            CheckoutViewModel model = await orderService.CheckoutCartItemsAsync();
            ViewData["ReCaptchaSiteKey"] = captchaSett.Value.SiteKey;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrderAndPay(CheckoutViewModel model,
            [FromServices] IOptions<StripeSettings> stripeOptions)
        {
            var token = Request.Form["g-recaptcha-response"];
            var ok = await captchaService.VerifyAsync(token);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "Please confirm you’re not a robot.");

                ViewData["ReCaptchaSiteKey"] = captchaSett.Value.SiteKey;
                return View("Checkout", model);
            }

            if (!ModelState.IsValid)
            {
                ViewData["ReCaptchaSiteKey"] = captchaSett.Value.SiteKey;
                return View("Checkout", model);
            }

            //Create Pending Order
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            Guid orderId = await orderService.PlaceOrderAsync(model, userId);


            //Map to Stripe line items
            PaymentViewModel? paymentVm = await orderService.GetPaymentViewModelAsync(orderId);
            if (paymentVm is null || !paymentVm.CartItems.Any())
                return RedirectToAction(nameof(Checkout));

            var lineItems = paymentVm.CartItems.Select(ci =>
                {
                    long unitAmount = (long)Math.Round(ci.Price * 100m, MidpointRounding.AwayFromZero);
                    return new SessionLineItemOptions
                    {
                        Quantity = ci.Quantity,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "eur",
                            UnitAmount = unitAmount,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = ci.ProductName,
                                Images = string.IsNullOrWhiteSpace(ci.ImageUrl)
                                    ? null
                                    : new List<string> { ci.ImageUrl }
                            }
                        }
                    };
                })
                .ToList();


            var baseUrl = Environment.GetEnvironmentVariable("APP__PUBLICBASEURL")
                          ?? $"{Request.Scheme}://{Request.Host}";
            var successBase = baseUrl + Url.Action(nameof(PaymentSuccess), "Order", new { orderId });
            var cancelUrl = baseUrl + Url.Action(nameof(PaymentCancel), "Order", new { orderId });
            var successSeparator = successBase.Contains('?') ? "&" : "?";
            var successUrl = successBase + successSeparator + "session_id={CHECKOUT_SESSION_ID}";

            var options = new SessionCreateOptions
            {
                Mode = "payment",
                PaymentMethodTypes = new List<String> { "card" },
                LineItems = lineItems,

                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,

                Metadata = new Dictionary<string, string>
                {
                    ["orderId"] = orderId.ToString(),
                    ["userId"] = userId
                }
            };

            try
            {
                var session = await new SessionService().CreateAsync(options);

                return Redirect(session.Url);
            }
            catch (StripeException ex)
            {
                Console.WriteLine("[Stripe] " + ex.Message);
                ModelState.AddModelError("", "We couldn't start the payment, Please try again.");
                return View("Checkout", model);
            }


        }

        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(Guid orderId,
            [FromQuery(Name = "session_id")] string sessionId)
        {

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                Console.WriteLine($"[PaymentSuccess] Missing session_id. Path={Request.Path}, Query={Request.QueryString}");

                TempData[ErrorMessageKey] = "Missing session_id on return from Stripe.";
                return RedirectToAction(nameof(PaymentCancel), new { orderId });
            }


            var session = await new SessionService().GetAsync(sessionId);
            if (String.IsNullOrEmpty(session.PaymentIntentId))
            {
                return RedirectToAction(nameof(PaymentCancel), new { orderId });
            }

            var pi = await new Stripe.PaymentIntentService().GetAsync(session.PaymentIntentId);
            if (pi.Status == "succeeded")
            {
                await orderService.MarkOrderAsPaidAsync(orderId);
                return RedirectToAction(nameof(ThankYou));
            }

            return RedirectToAction(nameof(PaymentCancel), new { orderId });
        }

        [HttpGet]
        public IActionResult PaymentCancel(Guid orderId)
        {
            return View(model: orderId);
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
