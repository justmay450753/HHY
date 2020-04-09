using HHY.Models;
using HHY.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HHY_NETCore.Provider
{
    public class TokenProvider
    {
        public string LoginUser(Members member)
        {
            //Authentication successful, Issue Token with user credentials
            //Provide the security key which was given in the JWToken configuration in Startup.cs
            var key = Encoding.ASCII.GetBytes
                        ("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
            //Generate Token for user 
            var JWToken = new JwtSecurityToken(
                issuer: "https://localhost:5001/",
                audience: "https://localhost:5001/",
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

        private List<Claim> GetUserClaims(Members member)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, member.Name),
                new Claim("Email", member.Email),
                new Claim("WRITE_ACCESS", "DIRECTOR"),
                new Claim("ACCESS_LEVEL", "DIRECTOR")
            };
            return claims;
        }
    }
}
