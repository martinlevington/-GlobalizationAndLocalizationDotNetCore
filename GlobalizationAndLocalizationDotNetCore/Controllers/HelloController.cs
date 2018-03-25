using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace GlobalisationAndLocalisationDotNetCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Hello")]
    public class HelloController : Controller
    {
        private readonly IHtmlLocalizer<HelloController> _htmlLocalizer;

        public HelloController(IHtmlLocalizer<HelloController> htmlLocalizer)
        {
            _htmlLocalizer = htmlLocalizer;
        }

        public IActionResult Hello(string name)
        {
            var textWriter = new StringWriter();

            _htmlLocalizer["<b>Hello</b><i> {0}</i>", name].WriteTo(textWriter, HtmlEncoder.Default);

            var result = new Dictionary<string, string> {{"response", textWriter.ToString()}};

            return Ok(result);
        }

    }
}