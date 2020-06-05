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
using System.IO;
using HHY_NETCore.Attributes;
using HHY.Models.Blog;

namespace HHY_NETCore.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly HHYContext _context;

        public BlogsController(HHYContext context)
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

        [HttpGet("api/admin/getblog")]
        public IActionResult Get(int? id)
        {
            if (id == null)
            {
                return Ok(_context.Blogs.ToList());
            }
            else
            {
                var Blogs = _context.Blogs.Where(r => r.ID == id);
                if (Blogs.Any())
                {
                    return Ok(Blogs.FirstOrDefault());
                }
                else
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
                }
            }
        }


        [HttpPost("api/admin/createblog")]
        public async Task<IActionResult> Create(RequestBlog blog)
        {
            try
            {
                if (blog.file != null)
                {
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", blog.file.FileName);
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await blog.file.CopyToAsync(stream);
                    }
                    blog.ImgUrl = "/Upload/" + blog.file.FileName;
                }

                var target = new Blogs()
                {
                    ImgUrl = blog.ImgUrl,
                    SubContent = blog.SubContent,
                    Type = Guid.Parse(blog.Type),
                    VideoUrl = blog.VideoUrl,
                    Content = blog.Content,
                    DownDate = blog.DownDate,
                    PublishDate = blog.PublishDate,
                    Title = blog.Title
                };

                _context.Blogs.Add(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPut("api/admin/editblog")]
        public async Task<IActionResult> Edit(RequestBlog blog)
        {
            var Blogs = _context.Blogs.Where(r => r.ID == blog.Id);
            if (Blogs.Any())
            {
                try
                {
                    var data = Blogs.FirstOrDefault();
                    data.Title = blog.Title;
                    data.VideoUrl = !string.IsNullOrEmpty(blog.VideoUrl) ? blog.VideoUrl : "";
                    data.SubContent = blog.SubContent;
                    data.Type = Guid.Parse(blog.Type);
                    data.Content = blog.Content;
                    data.PublishDate = blog.PublishDate;
                    data.DownDate = blog.DownDate;
                    if (blog.file != null)
                    {
                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Upload", blog.file.FileName);
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await blog.file.CopyToAsync(stream);
                        }
                        data.ImgUrl = "/Upload/" + blog.file.FileName;
                    }
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

        [HttpDelete("api/admin/delblog")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var about = _context.Blogs.Where(r => r.ID == id);
            if (about.Any())
            {
                _context.Blogs.Remove(about.FirstOrDefault());
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
