using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HHY.Models;
using HHY.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;
using HHY_NETCore.Attributes;

namespace HHY_NETCore.Controllers
{
    [Authorize]
    public class MenusController : Controller
    {
        private readonly HHYContext _context;

        public MenusController(HHYContext context)
        {
            _context = context;
        }

        #region View
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        #endregion

        [HttpGet("api/admin/getmenu")]
        public IActionResult Get(int? id)
        {
            if (id == null)
            {
                return Ok(_context.Menu.ToList());
            }
            else
            {
                var menus = _context.Menu.Where(r => r.ID == id);
                if (menus.Any())
                {
                    return Ok(menus.FirstOrDefault());
                }
                else
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
                }
            }
        }


        [HttpPost("api/admin/createmenu")]
        public async Task<IActionResult> Create(Menu menu)
        {
            try
            {
                _context.Menu.Add(menu);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPut("api/admin/editmenu")]
        public async Task<IActionResult> Edit(Menu menu)
        {
            var menus = _context.Menu.Where(r => r.ID == menu.ID);
            if (menus.Any())
            {
                try
                {
                    var data = menus.FirstOrDefault();
                    data.Title = menu.Title;
                    data.Url = menu.Url;
                    data.PublishDate = menu.PublishDate;
                    data.DownDate = menu.DownDate;
                    data.IsProductPromotion = menu.IsProductPromotion;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = ex.Message });
                }
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }

        [HttpDelete("api/admin/delmenu")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var menu = _context.Menu.Where(r => r.ID == id);
            if (menu.Any())
            {
                _context.Menu.Remove(menu.FirstOrDefault());
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }
    }
}
