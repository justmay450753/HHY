using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HHY.Models;
using HHY.Models;
using System.IO;
using static Microsoft.AspNetCore.Http.StatusCodes;
using HHY_NETCore.Attributes;
using HHY.Models.KeyVisual;

namespace HHY_NETCore.Controllers
{
    [Authorize]
    public class KeyVisualsController : Controller
    {
        private readonly HHYContext _context;

        public KeyVisualsController(HHYContext context)
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

        [HttpGet("api/admin/getKV")]
        public IActionResult Get(int? id)
        {
            if (id == null)
            {
                return Ok(_context.KeyVisuals.ToList());
            }
            else
            {
                var keyvisual = _context.KeyVisuals.Where(r => r.ID == id);
                if (keyvisual.Any())
                {
                    return Ok(keyvisual.FirstOrDefault());
                }
                else
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
                }
            }
        }

        [HttpPost("api/admin/createKV")]
        public async Task<IActionResult> Create(RequestKeyVisuals keyVisual)
        {
            if (keyVisual.file != null)
            {
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", keyVisual.file.FileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await keyVisual.file.CopyToAsync(stream);
                }
                keyVisual.ImgUrl = "/Upload/" + keyVisual.file.FileName;
            }

            try
            {
                var target = new KeyVisuals()
                {
                    DownDate = keyVisual.DownDate,
                    PublishDate = keyVisual.PublishDate,
                    Url = keyVisual.Url,
                    ImgUrl = keyVisual.ImgUrl,
                    Title = keyVisual.Title
                };

                _context.KeyVisuals.Add(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }

        [HttpPut("api/admin/editKV")]
        public async Task<IActionResult> Edit(RequestKeyVisuals keyVisual)
        {
            var KV = _context.KeyVisuals.Where(r => r.ID == keyVisual.Id);
            if (KV.Any())
            {
                try
                {
                    var data = KV.FirstOrDefault();
                    if (keyVisual.file != null)
                    {
                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", keyVisual.file.FileName);
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await keyVisual.file.CopyToAsync(stream);
                        }
                        data.ImgUrl = "/Upload/" + keyVisual.file.FileName;
                    }
                    data.Title = keyVisual.Title;
                    data.Url = keyVisual.Url;
                    data.PublishDate = keyVisual.PublishDate;
                    data.DownDate = keyVisual.DownDate;
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

        [HttpDelete("api/admin/delKV")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var keyVisual = _context.KeyVisuals.Where(r => r.ID == id);
            if (keyVisual.Any())
            {
                _context.KeyVisuals.Remove(keyVisual.FirstOrDefault());
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
