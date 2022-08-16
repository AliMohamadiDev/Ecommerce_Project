using System.Globalization;
using _0_Framework.Application;
using _0_Framework.Application.ZarinPal;
using _01_LampshadeQuery.Contracts;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages;

[Authorize]
public class CheckoutModel : PageModel
{
    public Cart Cart;
    public const string CookieName = "cart-items";

    private readonly IAuthHelper _authHelper;
    private readonly ICartService _cartService;
    private readonly IProductQuery _productQuery;
    private readonly IZarinPalFactory _zarinPalFactory;
    private readonly IOrderApplication _orderApplication;
    private readonly ICartCalculatorService _cartCalculatorService;

    public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService,
        IProductQuery productQuery, IOrderApplication orderApplication, IZarinPalFactory zarinPalFactory,
        IAuthHelper authHelper)
    {
        Cart = new();
        _authHelper = authHelper;
        _cartService = cartService;
        _productQuery = productQuery;
        _zarinPalFactory = zarinPalFactory;
        _orderApplication = orderApplication;
        _cartCalculatorService = cartCalculatorService;
    }

    public void OnGet()
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        var cartItems = serializer.Deserialize<List<CartItem>>(value);
        foreach (var item in cartItems)
            item.CalculateTotalItemPrice();

        Cart = _cartCalculatorService.ComputeCart(cartItems);
        _cartService.Set(Cart);
    }

    public IActionResult OnPostPay(int paymentMethod)
    {
        var cart = _cartService.Get();
        cart.SetPaymentMethod(paymentMethod);

        var result = _productQuery.CheckInventoryStatus(cart.Items);
        if (result.Any(x => !x.IsInStock))
            return RedirectToPage("/Cart");

        var orderId = _orderApplication.PlaceOrder(cart);
        if (cart.PaymentMethod == 1)
        {
            var paymentResponse =
                _zarinPalFactory.CreatePaymentRequest(cart.PayAmount.ToString(CultureInfo.InvariantCulture), "", "",
                    "خرید از درگاه پرداخت تستی", orderId);

            return Redirect($"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");
        }
        else
        {
            var paymentResult = new PaymentResult();
            return RedirectToPage("/PaymentResult",
                paymentResult.Succeeded("سفارش شما با موفقیت ثبت شد و پس از تماس کارشناسان ما ارسال خواهد شد.", null));
        }
    }

    public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status, [FromQuery] long oId)
    {
        var orderAmount = _orderApplication.GetAmountBy(oId);
        var verificationResponse = _zarinPalFactory.CreateVerificationRequest(authority, orderAmount.ToString(CultureInfo.InvariantCulture));

        var result = new PaymentResult();
        if (status == "OK" && verificationResponse.Status >= 100)
        {
            var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, verificationResponse.RefID);
            Response.Cookies.Delete(CookieName);
            result = result.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNo);
            return RedirectToPage("/PaymentResult", result);
        }
        else
        {
            result = result.Failed("پرداخت ناموفق بود.");
            return RedirectToPage("/PaymentResult", result);
        }
    }
}