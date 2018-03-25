using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalisationAndLocalisationDotNetCore.ViewModels;
using GlobalisationAndLocalisationDotNetCore.ViewModels.Info;
using Microsoft.AspNetCore.Mvc;

namespace GlobalisationAndLocalisationDotNetCore.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Index()
        {
            var vm = new InfoViewModel
            {
                Email = "test@test.com",
                Message = "This is a Msg",
                Title = "Test Title"
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(InfoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            return View(vm);

        }
    }
}