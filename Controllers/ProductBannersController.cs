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
using HHY.Models.ProductBanner;

namespace HHY_NETCore.Controllers
{
    [Authorize]
    public class ProductBannersController : Controller
    {
        private readonly HHYContext _context;

        public ProductBannersController(HHYContext context)
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

        [HttpGet("api/admin/getProductBanner")]
        public IActionResult Get(int? id)
        {
            if (id == null)
            {
                return Ok(_context.ProductBanners.ToList());
            }
            else
            {
                var productbanner = _context.ProductBanners.Where(r => r.ID == id);
                if (productbanner.Any())
                {
                    return Ok(productbanner.FirstOrDefault());
                }
                else
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
                }
            }
        }

        [HttpPost("api/admin/createProductBanner")]
        public async Task<IActionResult> Create(RequestProductBanner productBanner)
        {
            if (productBanner.file != null)
            {
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", productBanner.file.FileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await productBanner.file.CopyToAsync(stream);
                }
                productBanner.ImgUrl = "/Upload/" + productBanner.file.FileName;
            }

            try
            {
                var targer = new ProductBanners()
                {
                    ImgUrl = productBanner.ImgUrl,
                    PublishDate = productBanner.PublishDate,
                    Url = productBanner.Url,
                    DownDate = productBanner.DownDate,
                    Title = productBanner.Title
                };
                _context.ProductBanners.Add(targer);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }

        [HttpPut("api/admin/editProductBanner")]
        public async Task<IActionResult> Edit(RequestProductBanner productBanner)
        {
            var ProductBanner = _context.ProductBanners.Where(r => r.ID == productBanner.ID);
            if (ProductBanner.Any())
            {
                try
                {
                    var data = ProductBanner.FirstOrDefault();
                    if (productBanner.file != null)
                    {
                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", productBanner.file.FileName);
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await productBanner.file.CopyToAsync(stream);
                        }
                        data.ImgUrl = "/Upload/" + productBanner.file.FileName;
                    }
                    data.Title = productBanner.Title;
                    data.Url = productBanner.Url;
                    data.PublishDate = productBanner.PublishDate;
                    data.DownDate = productBanner.DownDate;
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

        [HttpDelete("api/admin/delProductBanner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var productBanner = _context.ProductBanners.Where(r => r.ID == id);
            if (productBanner.Any())
            {
                _context.ProductBanners.Remove(productBanner.FirstOrDefault());
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
