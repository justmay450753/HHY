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
using HHY.Models.SeasonalActivites;

namespace HHY_NETCore.Controllers
{
    [Authorize]
    public class SeasonalActivitiesController : Controller
    {
        private readonly HHYContext _context;

        public SeasonalActivitiesController(HHYContext context)
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

        [HttpGet("api/admin/getseasonalactivities")]
        public IActionResult Get(int? id)
        {
            if (id == null)
            {
                return Ok(_context.SeasonalActivities.ToList());
            }
            else
            {
                var seasonalActivity = _context.SeasonalActivities.Where(r => r.ID == id);
                if (seasonalActivity.Any())
                {
                    return Ok(seasonalActivity.FirstOrDefault());
                }
                else
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
                }
            }
        }

        [HttpPost("api/admin/createseasonalactivities")]
        public async Task<IActionResult> Create(RequestSeasonalActivities seasonalActivity)
        {
            if (seasonalActivity.file != null)
            {
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", seasonalActivity.file.FileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await seasonalActivity.file.CopyToAsync(stream);
                }
                seasonalActivity.ImgUrl = "/Upload/" + seasonalActivity.file.FileName;
            }

            try
            {
                var target = new SeasonalActivities() { 
                    Url = seasonalActivity.Url,
                    ImgUrl = seasonalActivity.ImgUrl,
                    DownDate = seasonalActivity.DownDate,
                    PublishDate = seasonalActivity.PublishDate,
                    Title = seasonalActivity.Title
                };

                _context.SeasonalActivities.Add(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }

        [HttpPut("api/admin/editseasonalactivities")]
        public async Task<IActionResult> Edit(RequestSeasonalActivities seasonalActivity)
        {
            var seasonal = _context.SeasonalActivities.Where(r => r.ID == seasonalActivity.Id);
            if (seasonal.Any())
            {
                try
                {
                    var data = seasonal.FirstOrDefault();
                    if (seasonalActivity.file != null)
                    {
                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", seasonalActivity.file.FileName);
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await seasonalActivity.file.CopyToAsync(stream);
                        }
                        data.ImgUrl = "/Upload/" + seasonalActivity.file.FileName;
                    }
                    data.Title = seasonalActivity.Title;
                    data.Url = seasonalActivity.Url;
                    data.PublishDate = seasonalActivity.PublishDate;
                    data.DownDate = seasonalActivity.DownDate;
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

        [HttpDelete("api/admin/delseasonalactivities")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var seasonalActivity = _context.SeasonalActivities.Where(r => r.ID == id);
            if (seasonalActivity.Any())
            {
                _context.SeasonalActivities.Remove(seasonalActivity.FirstOrDefault());
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
