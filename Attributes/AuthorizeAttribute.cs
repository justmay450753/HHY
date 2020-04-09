using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HHY_NETCore.Attributes
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(params string[] claim) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { claim };
        }
    }

    public class AuthorizeFilter : IAuthorizationFilter
    {
        readonly string[] _claim;

        public AuthorizeFilter(params string[] claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var IsAuthenticated = context.HttpContext.Session.GetString("JWToken") != null ? true : false;
            var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;

            if (IsAuthenticated)
            {
                //bool flagClaim = false;
                //foreach (var item in _claim)
                //{
                //    if (context.HttpContext.User.HasClaim(item, item))
                //        flagClaim = true;
                //}
                //if (!flagClaim)

                //context.HttpContext.Response.Redirect("~/Members/Login");
            }
            else
            {
                context.HttpContext.Response.Redirect("/Index/Login");
                //SetErrorResponse(context, "尚未登入");
            }
            return;
        }

        private void SetErrorResponse(AuthorizationFilterContext context, string Message)
        {
            context.Result = new JsonResult(new ResponseJson() { Message = Message});
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            context.HttpContext.Response.Redirect("/Index/Login");
            return;
        }

        public class ResponseJson
        {
            public string Message { get; set; }
        }
    }
}
