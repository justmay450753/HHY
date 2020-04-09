using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HHY.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;
using HHY_NETCore.Attributes;

namespace HHY_NETCore.Controllers
{
    [Authorize]
    public class AboutsController : Controller
    {
        private readonly HHYContext _context;

        public AboutsController(HHYContext context)
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

        [HttpGet("api/admin/getabout")]
        public IActionResult Get(int? id)
        {
            if (id == null)
            {
                return Ok(_context.Abouts.ToList());
            }
            else
            {
                var abouts = _context.Abouts.Where(r => r.ID == id);
                if (abouts.Any())
                {
                    return Ok(abouts.FirstOrDefault());
                }
                else
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
                }
            }
        }


        [HttpPost("api/admin/createabout")]
        public async Task<IActionResult> Create(Abouts about)
        {
            try
            {
                _context.Abouts.Add(about);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPut("api/admin/editabout")]
        public async Task<IActionResult> Edit(Abouts about)
        {
            var abouts = _context.Abouts.Where(r => r.ID == about.ID);
            if (abouts.Any())
            {
                try
                {
                    var data = abouts.FirstOrDefault();
                    data.Title = about.Title;
                    data.Content = about.Content;
                    data.PublishDate = about.PublishDate;
                    data.DownDate = about.DownDate;
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

        [HttpDelete("api/admin/delabout")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var about = _context.Abouts.Where(r => r.ID == id);
            if (about.Any())
            {
                _context.Abouts.Remove(about.FirstOrDefault());
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
