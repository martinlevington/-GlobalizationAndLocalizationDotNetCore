﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GlobalisationAndLocalisationDotNetCore.Controllers
{
    [Route("api/[controller]")]
    public class AboutController : Controller
    {
        private readonly IStringLocalizer<AboutController> _localizer;
        public AboutController(IStringLocalizer<AboutController> localizer)
        {
            _localizer = localizer;
        }

      [HttpGet]
      public string Get()
        {
            return _localizer["About Title"];
        }
    }
}