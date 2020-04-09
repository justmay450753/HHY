using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HHY_NETCore.Controllers
{
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult BlogContent()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("JWToken") != null)
            {
                return Redirect("/Members/Index");
            }
            else
            {

                return View();
            }
        }

        public IActionResult Forget()
        {
            return View();
        }
    }
}