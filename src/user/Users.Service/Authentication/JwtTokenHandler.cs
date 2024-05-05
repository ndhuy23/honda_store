using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims; 
using System.Text;
using System.Threading.Tasks;
using Users.Data.DataAccess;
using Users.Data.ViewModels.Dtos;
using Users.Data.ViewModels.Response;

namespace Users.Service.Authentication
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "awdkKAaSDsfaDFsdfSawrDKASDsuEgFKAsdaDoAsdFWwAkd129fASDKasfASL";

        private const int JWT_TOKEN_VALIDITY_MINS = 30;
        private readonly UserDbContext _db;

        public JwtTokenHandler(UserDbContext db)
        {
            _db = db;
        }
        public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password)) return null;

            //Validation

            var userAccount = _db.Users.Where(us => us.UserName == request.UserName
                                                   && us.Password == request.Password).FirstOrDefault();
            if (userAccount == null) return null;

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, request.UserName),
                new Claim("Role", userAccount.Role.ToString())
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                UserName = userAccount.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token
            };
                        
            

        }
    }
}
