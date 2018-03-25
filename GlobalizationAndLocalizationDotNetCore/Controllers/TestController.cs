using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GlobalisationAndLocalisationDotNetCore.Controllers
{
    public class TestController : Controller
    {
        private readonly IStringLocalizer _localizer;
        private readonly IStringLocalizer _localizerShared;


        public TestController(IStringLocalizer<TestController> localizer, IStringLocalizerFactory factory)
        {
          
            // get resource from shared dll file
            _localizerShared = factory.Create("Controllers.TestController", "GlobalisationAndLocalisationResources");
            // get resource from local file
            _localizer = localizer;
        }


        public IActionResult Index()
        {
            ViewData["Message"] = "Local Hello:" + _localizer["Hello"] + "<br/>" 
                                + "Shared Hello:" + _localizerShared["Hello"];

            return View();
        }
    }
}