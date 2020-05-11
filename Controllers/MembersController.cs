using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HHY.Models;
using HHY_NETCore.Provider;
using Microsoft.AspNetCore.Http;
using HHY_NETCore.Attributes;
using static Microsoft.AspNetCore.Http.StatusCodes;
using System.Text.RegularExpressions;
using HHY_NETCore.Extension;
using HHY.Models.View;
using HHY.Models.Member;

namespace HHY_NETCore.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly HHYContext _context;

        public MembersController(HHYContext context)
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

        
        [HttpGet("api/admin/membergetmember")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return Ok(await _context.Members.ToListAsync());
            }
            else
            {
                var member = _context.Members.Where(r => r.ID == id);
                if (member.Any())
                {
                    return Ok(await member.FirstOrDefaultAsync());
                }
                else
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
                }
            }
        }

        
        [HttpGet("api/admin/memberlogout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return Redirect("~/Members/Login");
        }

        
        [HttpPost("api/admin/membercreate")]
        public async Task<IActionResult> Create(RequestMembers member)
        {
            Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{10,30}$");

            if (!regex.IsMatch(member.Password))
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "字串長度在 10 ~ 30 個字母之間，且至少一個小寫英文字母、大寫英文字母和數字。" });
            }

            if (member.Password != member.CheckPassword)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "密碼與確認密碼不符合" });
            }
            member.Password = StringEncryptExtension.aesEncryptBase64(member.Password, member.Email);

            try
            {
                var mem = new Members()
                {
                    Email = member.Email,
                    IsVerify = member.IsVerify,
                    Name = member.Name,
                    Password = member.Password
                };
                _context.Add(mem);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "註冊失敗" });
            }
        }

        
        [HttpPut("api/admin/memberedit")]
        public async Task<IActionResult> Edit(RequestEditMember member)
        {
            var edit = _context.Members.Where(r => r.ID == member.ID);
            Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{10,30}$");

            if (edit.Any())
            {
                var data = edit.FirstOrDefault();
                if(data.Password != StringEncryptExtension.aesEncryptBase64(member.OldPassword, data.Email))
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "舊密碼輸入錯誤" });
                }

                if (!regex.IsMatch(member.Password))
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "字串長度在 10 ~ 30 個字母之間，且至少一個小寫英文字母、大寫英文字母和數字。" });
                }

                if (member.Password != member.chkPassword)
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "密碼與確認密碼不符合" });
                }

                try
                {
                    data.Password = StringEncryptExtension.aesEncryptBase64(member.Password, data.Email);
                    data.Name = member.Name;
                    data.IsVerify = member.IsVerify;
                    await _context.SaveChangesAsync();
                    if (!string.IsNullOrEmpty(member.OldPassword))
                    {
                        HttpContext.Session.Remove("JWToken");
                    }
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(Status400BadRequest, new ResponseMessage { Message = "修改資料失敗" });
                }
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資料" });
            }
        }

        
        [HttpDelete("api/admin/memberdelete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "查無資訊" });
            }

            var member = _context.Members.Where(r => r.ID == id && r.IsVerify == false);
            if (member.Any())
            {
                _context.Members.Remove(member.FirstOrDefault());
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(Status400BadRequest, new ResponseMessage { Message = "無法刪除" });
            }
        }
    }
}
