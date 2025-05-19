using System.Text.Json;
using TakeAway.ViewModels;

namespace TakeAway.Utilities
{
    public static class CookieHelper
    {
        public static void SetBasketCookie(HttpResponse response, BasketViewModel basket)
        {
            string json = JsonSerializer.Serialize(basket);
            response.Cookies.Append("basket", json, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(7) });
        }

        public static BasketViewModel GetBasketFromCookie(HttpRequest request)
        {
            if (request.Cookies.TryGetValue("basket", out string json))
            {
                return JsonSerializer.Deserialize<BasketViewModel>(json) ?? new BasketViewModel();
            }
            else
            {
                return new BasketViewModel();
            }

        }
    }
}
