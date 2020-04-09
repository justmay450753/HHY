using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HHY_NETCore.Controllers.Views
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult GetProductList()
        {
            return Ok();
        }
    }
}