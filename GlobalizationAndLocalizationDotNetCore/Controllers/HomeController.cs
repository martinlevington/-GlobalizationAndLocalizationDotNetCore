using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using GlobalisationAndLocalisationDotNetCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace GlobalisationAndLocalisationDotNetCore.Controllers
{
    public class HomeController : Controller
    {

        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IOptions<RequestLocalizationOptions> _localizationOptions;

        public HomeController(IStringLocalizer<HomeController> localizer, IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _localizer = localizer;
            _localizationOptions = localizationOptions;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = _localizer["Your application description page."];

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = _localizer["Your contact page."];

            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            var currentCulture = _localizationOptions.Value.DefaultRequestCulture.Culture.Name;
            if (RouteData.Values.ContainsKey("culture"))
            {
                currentCulture = (string)RouteData.Values["culture"];
            }

        
                returnUrl = ReplaceFirst(returnUrl, currentCulture, culture);
            

            return LocalRedirect(returnUrl);
        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
