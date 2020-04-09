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
using HHY.Models.Product;

namespace HHY_NETCore.Controllers
{
    [Authorize]
    public class HotSaleProductsController : Controller
    {
        private readonly HHYContext _context;

        public HotSaleProductsController(HHYContext context)
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

        [HttpGet("api/admin/getallproduct")]
        public IActionResult GetProducts()
        {
            var products = _context.Products;
            if (products.Any())
            {
                return Ok(products.Select(r => new ProductSelect() { 
                    Item = r.Name + " " + r.Weight,
                    ID = r.ID.ToString()
                }));
            }
            else
            {
                return Ok(new ProductSelect() { });
            }
        }

        [HttpGet("api/admin/gethotsale")]
        public IActionResult Get(int? id)
        {
            if (id == null)
            {
                var result = from r in _context.HotSaleProducts
                             join s in _context.Products on r.ProductID equals s.ID
                             select new HotSaleProductView()
                             {
                                 ID = r.ID,
                                 ProductID = s.ID.ToString(),
                                 AdvertisingLine = r.AdvertisingLine,
                                 ProductInfo = s.Name + " " + s.Weight,
                                 Url = r.Url,
                                 DownDate = r.DownDate,
                                 PublishDate = r.PublishDate
                             };

                return Ok(result);
            }
            else
            {
                var hotsales = _context.HotSaleProducts.Where(r => r.ID == id);
                if (hotsales.Any())
                {
                    var result = from r in _context.HotSaleProducts
                                 join s in _context.Products on r.ProductID equals s.ID
                                 where r.ID == id
                                 select new HotSaleProductView()
                                 {
                                     ID = r.ID,
                                     ProductID = s.ID.ToString(),
                                     AdvertisingLine = r.AdvertisingLine,
                                     ProductInfo = s.Name + " " + s.Weight,
                                     Url = r.Url,
                                     DownDate = r.DownDate,
                                     PublishDate = r.PublishDate
                                 };

                    return Ok(result.FirstOrDefault());
                }
                else
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
                }
            }
        }


        [HttpPost("api/admin/createhotsale")]
        public async Task<IActionResult> Create(HotSaleProducts product)
        {
            try
            {
                product.Url = "/product/detail?id=" + product.ProductID;
                _context.HotSaleProducts.Add(product);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPut("api/admin/edithotsale")]
        public async Task<IActionResult> Edit(HotSaleProducts hotsale)
        {
            var hotsales = _context.HotSaleProducts.Where(r => r.ID == hotsale.ID);
            if (hotsales.Any())
            {
                try
                {
                    var data = hotsales.FirstOrDefault();
                    data.ProductID = hotsale.ProductID;
                    data.AdvertisingLine = hotsale.AdvertisingLine;
                    data.Url = "/product/detail?id=" + hotsale.ProductID;
                    data.PublishDate = hotsale.PublishDate;
                    data.DownDate = hotsale.DownDate;
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

        [HttpDelete("api/admin/delhotsale")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var about = _context.HotSaleProducts.Where(r => r.ID == id);
            if (about.Any())
            {
                _context.HotSaleProducts.Remove(about.FirstOrDefault());
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
