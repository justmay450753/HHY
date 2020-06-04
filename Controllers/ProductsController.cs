using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HHY.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;
using System.IO;
using HHY_NETCore.Attributes;
using HHY.Models.Product;

namespace HHY_NETCore.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly HHYContext _context;

        public ProductsController(HHYContext context)
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

        [HttpGet("api/admin/getproduct")]
        public IActionResult Get(string id)
        {
            if (id == null)
            {
                return Ok(_context.Products.ToList());
            }
            else
            {
                var products = _context.Products.Where(r => r.ID == Guid.Parse(id));
                if (products.Any())
                {
                    var result = products.Select(r => new ProductView()
                    {
                        ID = r.ID.ToString(),
                        BuyUrl = r.BuyUrl,
                        SubTitle = r.SubTitle,
                        DownDate = r.DownDate,
                        ImgUrl = r.ImgUrl,
                        Information = r.Information,
                        IsPromotion = r.IsPromotion,
                        Name = r.Name,
                        Price = r.Price,
                        PublishDate = r.PublishDate,
                        SalePrice = r.SalePrice,
                        Weight = r.Weight,
                        Standard = r.Standard,
                        ReportFiles = _context.ReportFile.Where(s => s.ProductID == r.ID).ToList()
                    }).FirstOrDefault();
                    return Ok(result);
                }
                else
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
                }
            }
        }


        [HttpPost("api/admin/createproduct")]
        public async Task<IActionResult> Create(RequestPD product)
        {
            var ProductID = Guid.NewGuid();
            product.ID = ProductID.ToString();
            var reportFiles = new List<ReportFile>();
            if (product.file != null)
            {
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", product.file.FileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await product.file.CopyToAsync(stream);
                }
                product.ImgUrl = "/Upload/" + product.file.FileName;
            }

            if (product.Reportfile != null)
            {
                foreach (var item in product.Reportfile)
                {
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", item.FileName);
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    reportFiles.Add(new ReportFile()
                    {
                        ProductID = ProductID,
                        FileName = item.FileName,
                        Path = "/Upload/" + item.FileName
                    });

                    _context.ReportFile.AddRange(reportFiles);
                    await _context.SaveChangesAsync();
                }
            }

            try
            {
                var target = new Products()
                {
                    ID = ProductID,
                    BuyUrl = product.BuyUrl,
                    ImgUrl = product.ImgUrl,
                    DownDate = product.DownDate,
                    Information = product.Information,
                    IsPromotion = product.IsPromotion,
                    Name = product.Name,
                    Price = product.Price,
                    PublishDate = product.PublishDate,
                    SalePrice = product.SalePrice.Value,
                    SubTitle = product.SubTitle,
                    Weight = product.Weight,
                    Standard = product.Standard
                };
                _context.Products.Add(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }

        [HttpPut("api/admin/editproduct")]
        public async Task<IActionResult> Edit(RequestPD product)
        {
            var reportFiles = new List<ReportFile>();
            var products = _context.Products.Where(r => r.ID == Guid.Parse(product.ID));
            if (products.Any())
            {
                try
                {
                    var data = products.FirstOrDefault();
                    if (product.file != null)
                    {
                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", product.file.FileName);
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await product.file.CopyToAsync(stream);
                        }
                        data.ImgUrl = "/Upload/" + product.file.FileName;
                    }

                    if (product.Reportfile != null)
                    {
                        foreach (var item in product.Reportfile)
                        {
                            var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", item.FileName);
                            using (var stream = new FileStream(savePath, FileMode.Create))
                            {
                                await item.CopyToAsync(stream);
                            }

                            reportFiles.Add(new ReportFile()
                            {
                                ProductID = Guid.Parse(product.ID),
                                FileName = item.FileName,
                                Path = "/Upload/" + item.FileName
                            });
                        }

                        _context.ReportFile.AddRange(reportFiles);
                        _context.SaveChanges();
                    }
                    data.Name = product.Name;
                    data.Weight = product.Weight;
                    data.Price = product.Price;
                    data.SalePrice = product.SalePrice.Value;
                    data.Information = product.Information;
                    data.PublishDate = product.PublishDate;
                    data.DownDate = product.DownDate;
                    data.BuyUrl = product.BuyUrl;
                    data.Standard = product.Standard;
                    data.SubTitle = product.SubTitle;
                    data.IsPromotion = product.IsPromotion;

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

        [HttpDelete("api/admin/delproduct")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var about = _context.Products.Where(r => r.ID == Guid.Parse(id));
            if (about.Any())
            {
                _context.Products.Remove(about.FirstOrDefault());
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }

        [HttpDelete("api/admin/delproductfile")]
        public async Task<IActionResult> DeleteFile(string ProductID, string ID)
        {
            if (ID == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var about = _context.ReportFile.Where(r => r.ProductID == Guid.Parse(ProductID) && r.ID.ToString() == ID);
            if (about.Any())
            {
                _context.ReportFile.Remove(about.FirstOrDefault());
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
