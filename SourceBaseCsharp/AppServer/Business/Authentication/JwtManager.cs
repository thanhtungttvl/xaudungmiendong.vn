using AppShare.Core;
using AppShare.Models.User;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppServer.Business.Authentication
{
    public class JwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Tạo token login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserModel GenerateJwtToken(UserModel model)
        {
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]!));

            var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            //var tokenKey = Convert.FromHexString(_configuration["Jwt:Key"]!);

            var claims = new List<Claim>
            {
                new Claim("Id", model.Id.ToString()),
                new Claim("Name", model.Name),
                new Claim(ClaimTypes.Role, model.Role.GetDescription()),
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials,
                Issuer = _configuration["Jwt:Issuer"]!,
                Audience = _configuration["Jwt:Audience"]!,
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
                var token = jwtSecurityTokenHandler.WriteToken(securityToken);

                model.Token = token;
                model.ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds;
                model.ExpiryTimeStamp = DateTime.Now.AddSeconds(model.ExpiresIn);

                return model;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Check login bằng token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public JwtSecurityToken? ValidateJwtToken(string? token)
        {
            if (token is null)
            {
                return null;
            }

            var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            //var tokenKey = Convert.FromHexString(_configuration["Jwt:Key"]!);

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),

                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"]!,

                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"]!,

                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                // return user id from JWT token if validation successful
                return validatedToken as JwtSecurityToken;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
