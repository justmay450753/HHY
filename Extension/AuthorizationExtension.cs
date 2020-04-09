using HHY.Models;
using HHY.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HHY_NETCore.Extension
{
    public class AuthorizationExtension
    {
        readonly HHYContext _context;
        public AuthorizationExtension(HHYContext context)
        {
            _context = context;
        }
        public string LoginUser(string strEmail, string strPassword)
        {
            //Get user details for the user who is trying to login

            var member = _context.Members.SingleOrDefault(r => r.Email == strEmail && r.Password == strPassword);

            //Authenticate User, Check if it’s a registered user in Database
            if (member != null)
            { 
                //Authentication successful, Issue Token with user credentials
                //Provide the security key which was given in the JWToken configuration in Startup.cs
                var key = Encoding.ASCII.GetBytes
                          ("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
                //Generate Token for user 
                var JWToken = new JwtSecurityToken(
                    issuer: "http://localhost:45092/",
                    audience: "http://localhost:45092/",
                    claims: GetUserClaims(member),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                    //Using HS256 Algorithm to encrypt Token
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha256Signature)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                return token;
            }
            else
            {
                return null;
            }
        }

        private List<Claim> GetUserClaims(Members member)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, member.Name),
                new Claim("Email", member.Email),
            };
            return claims;
        }
    }
}
