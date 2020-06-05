using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HHY.Models;
using Microsoft.AspNetCore.Mvc;
using HHY.Models.View;
using HHY.Models;
using LinqKit;
using static Microsoft.AspNetCore.Http.StatusCodes;
using HHY_NETCore.Extension;
using HHY_NETCore.Provider;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MailKit.Net.Smtp;

namespace HHY_NETCore.Controllers
{
    public class DefaultController : Controller
    {
        private readonly HHYContext _context;

        public DefaultController(HHYContext context)
        {
            _context = context;
        }

        [HttpPost("api/admin/memberlogin")]
        public IActionResult Login(RequestLogin login)
        {
            login.Password = StringEncryptExtension.aesEncryptBase64(login.Password, login.Account);
            var target = _context.Members.SingleOrDefault(r => r.Email == login.Account && r.Password == login.Password && r.IsVerify == true);
            if (target != null)
            {
                TokenProvider _tokenProvider = new TokenProvider();
                //Authenticate user
                var userToken = _tokenProvider.LoginUser(target);
                if (userToken != null)
                {
                    //Save token in session object
                    HttpContext.Session.SetString("JWToken", userToken);
                }
                return Redirect("~/Members/Index");
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "登入失敗" });
            }
        }

        [HttpGet("api/admin/memberforget")]
        public async Task<IActionResult> Forget(string Account)
        {
            var member = _context.Members.Where(r => r.Email == Account);
            if (member.Any())
            {
                string newPassword = RandomPasswordExtension.CreateRandomPassword();
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("胡家幸福蜂蜜管理人員", "just450753_may@hotmail.com"));
                    message.To.Add(new MailboxAddress(member.FirstOrDefault().Name, member.FirstOrDefault().Email));
                    message.Subject = "忘記密碼重設";

                    message.Body = new TextPart("plain")
                    {
                        Text = @"你好，你的新密碼為" + newPassword + "，請使用此密碼重新登入並重設密碼。"
                    };

                    using (var client = new SmtpClient())
                    {
                        // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        client.Connect("smtp.office365.com", 587, false);

                        // Note: only needed if the SMTP server requires authentication
                        client.Authenticate("just450753_may@hotmail.com", "RGIRaal2");

                        await client.SendAsync(message);
                        client.Disconnect(true);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "發信失敗" });
                }

                member.FirstOrDefault().Password = StringEncryptExtension.aesEncryptBase64(newPassword, member.FirstOrDefault().Email);
                await _context.SaveChangesAsync();
            }


            return Ok();
        }

        [HttpGet("api/web/menus")]
        public IActionResult GetMenus()
        {
            var MenuContents = new List<MenuView>();

            #region Menu
            var menus = _context.Menu.Where(r => r.PublishDate <= DateTime.Now &&
                            r.DownDate >= DateTime.Now);
            if (menus.Any())
            {
                MenuContents = menus.Select(r => new MenuView()
                {
                    Title = r.Title,
                    Link = r.Url,
                    IsProductPromotion = r.IsProductPromotion
                }).ToList();
            }
            #endregion

            return Ok(MenuContents);
        }

        [HttpGet("api/web/footers")]
        public IActionResult GetFooter()
        {
            var AboutContents = new List<AboutView>();

            #region 靜態頁面
            var About = _context.Abouts.Where(r => r.PublishDate <= DateTime.Now &&
                            r.DownDate >= DateTime.Now);
            if (About.Any())
            {
                AboutContents = About.Select(r => new AboutView()
                {
                    Title = r.Title,
                    Link = "/Index/About?id=" + r.ID
                }).ToList();
            }
            #endregion

            return Ok(AboutContents);
        }

        [HttpGet("api/web/default")]
        public IActionResult GetDefault()
        {
            var ActivityContents = new List<SeasonalActivityVIew>();
            var KeyVisualContents = new List<KVView>();
            var HotSaleContents = new List<HotSaleView>();
            var BlogContents = new List<BlogView>();
            var MenuContents = new List<MenuView>();
            var AboutContents = new List<AboutView>();

            #region Menu
            var menus = _context.Menu.Where(r => r.PublishDate <= DateTime.Now &&
                            r.DownDate >= DateTime.Now);
            if (menus.Any())
            {
                MenuContents = menus.Select(r => new MenuView()
                {
                    Title = r.Title,
                    Link = r.Url,
                    IsProductPromotion = r.IsProductPromotion
                }).ToList();
            }
            #endregion

            #region 輪播
            var KeyVisual = _context.KeyVisuals.Where(r => r.PublishDate <= DateTime.Now &&
                            r.DownDate >= DateTime.Now);
            if (KeyVisual.Any())
            {
                KeyVisualContents = KeyVisual.Take(3).Select(r => new KVView()
                {
                    Content = @"<a href= '" + r.Url + "'><img src ='" + r.ImgUrl + @"'alt ='" + r.Title + @"'></a>"
                }).ToList();
            }
            #endregion

            #region 活動跑馬燈
            var Activities = _context.SeasonalActivities.Where(r => r.PublishDate <= DateTime.Now &&
                            r.DownDate >= DateTime.Now);
            if (KeyVisual.Any())
            {
                ActivityContents = Activities.Take(3).Select(r => new SeasonalActivityVIew()
                {
                    Content = @"<a href= '" + r.Url + "'><img src= '/images/bell.svg'/>" + r.Title + "</a>"
                }).ToList();
            }
            #endregion

            #region 熱銷商品
            var HotSale = from r in _context.HotSaleProducts
                          join s in _context.Products on r.ProductID equals s.ID
                          where r.PublishDate <= DateTime.Now
                          where r.DownDate >= DateTime.Now
                          where s.PublishDate <= DateTime.Now
                          where s.DownDate >= DateTime.Now
                          orderby r.PublishDate descending
                          select new HotSaleView()
                          {
                              Link = "/Product/Detail?ID=" + s.ID,
                              OrignalPrice = Convert.ToInt32(s.Price),
                              SalePrice = Convert.ToInt32(s.SalePrice),
                              ProductImage = s.ImgUrl,
                              ProductName = s.Name,
                              SLOGN = r.AdvertisingLine,
                              Weight = s.Weight
                          };
            if (HotSale.Any())
            {
                HotSaleContents = HotSale.Take(3).ToList();
            }

            #endregion

            #region 蜂蜜小學堂
            var Blog = _context.Blogs.Where(r => r.PublishDate <= DateTime.Now &&
                            r.DownDate >= DateTime.Now);
            if (Blog.Any())
            {
                BlogContents = Blog.Take(4).Select(r => new BlogView()
                {
                    Link = "/Index/BlogContent?id=" + r.ID,
                    Image = r.ImgUrl,
                    Title = r.Title,
                    SubContent = r.SubContent
                }).ToList();
            }
            #endregion

            #region 靜態頁面
            var About = _context.Abouts.Where(r => r.PublishDate <= DateTime.Now &&
                            r.DownDate >= DateTime.Now);
            if (About.Any())
            {
                AboutContents = About.Select(r => new AboutView()
                {
                    Title = r.Title,
                    Link = "About/ID=" + r.ID
                }).ToList();
            }
            #endregion

            return Ok(new DefaultView()
            {
                Menus = MenuContents,
                KeyVisuals = KeyVisualContents,
                SeasonalActivities = ActivityContents,
                Blogs = BlogContents,
                HotSales = HotSaleContents
            });
        }

        [HttpGet("api/web/about")]
        public IActionResult GetAboutContent(string ID)
        {
            var about = _context.Abouts.Where(r => r.ID == int.Parse(ID));
            if (about.Any())
            {
                return Ok(about.FirstOrDefault());
            }
            else
            {
                return Ok(new Abouts() { });
            }
        }

        [HttpPost("api/web/productlist")]
        public IActionResult GetProudctList(RequestProduct request)
        {
            var search = PredicateBuilder.New<Products>();

            if (request.IsPromotion == true)
            {
                search.And(r => r.IsPromotion == request.IsPromotion);
            }

            search.And(r => r.PublishDate <= DateTime.Now && r.DownDate >= DateTime.Now);

            var products = _context.Products.Where(search);
            if (products.Any())
            {
                var result = products.Select(r => new ProductListView()
                {
                    ProductID = r.ID.ToString(),
                    ProductName = r.Name,
                    Link = "/Product/Detail?id=" + r.ID,
                    ImageUrl = r.ImgUrl,
                    OriganalPrice = r.Price,
                    SalePrice = r.SalePrice,
                    Weight = r.Weight
                });

                return Ok(result);
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無商品資訊" });
            }

        }

        [HttpGet("api/web/productdetail")]
        public IActionResult GetProductDetail(string ID)
        {
            var result = _context.Products.Where(r => r.ID == Guid.Parse(ID));

            if (result.Any())
            {
                return Ok(
                    result.Select(r => new ProductDetailView()
                    {
                        ImageUrl = r.ImgUrl,
                        BuyLink = r.BuyUrl,
                        OriganalPrice = r.Price,
                        SalePrice = r.SalePrice,
                        ProductInfo = r.Information,
                        ProductName = r.Name,
                        Weight = r.Weight,
                        Standard = r.Standard,
                        SubTitle = r.SubTitle,
                        ReportFiles = _context.ReportFile.Where(s => s.ProductID == Guid.Parse(ID)).Select(s => s.Path).ToList()
                    }).FirstOrDefault()
                );
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

        }

        [HttpGet("api/web/productbanner")]
        public IActionResult GetProductBanner()
        {
            var result = _context.ProductBanners.Where(r => r.PublishDate <= DateTime.Now &&
                            r.DownDate >= DateTime.Now);

            if (result.Any())
            {
                return Ok(
                    result.Select(r => new BannerView()
                    {
                        ImageUrl = r.ImgUrl,
                        Link = r.Url
                    }).FirstOrDefault()
                );
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }

        [HttpGet("api/web/blogcontent")]
        public IActionResult GetBlogContent(string ID)
        {
            var result = _context.Blogs.Where(r => r.ID.ToString() == ID);

            if (result.Any())
            {
                return Ok(
                    result.Select(r => new BlogContent()
                    {
                        Title = r.Title,
                        SubContent = r.SubContent,
                        Content = r.Content,
                        VideoUrl = !string.IsNullOrEmpty(r.VideoUrl) ? 
                                    @"<iframe width='560' height='315' src='https://www.youtube.com/embed/" + r.VideoUrl + @"' frameborder='0'
                                        allow = 'accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen >
                                    </iframe>" : ""
                    }).FirstOrDefault()
                );
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }

        [HttpGet("api/web/bloglist")]
        public IActionResult GetBlogList()
        {
            var result = _context.Blogs.Where(r => r.PublishDate <= DateTime.Now &&
                            r.DownDate >= DateTime.Now);

            if (result.Any())
            {
                return Ok(
                    result.Select(r => new BlogView()
                    {
                        Title = r.Title,
                        SubContent = r.SubContent,
                        Image = r.ImgUrl,
                        Type = r.Type.ToString(),
                        Link = "BlogContent?id=" + r.ID
                    }).ToList()
                );
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }
        }
    }
}