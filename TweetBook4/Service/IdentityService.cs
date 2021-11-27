using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TweetBook4.Data;
using TweetBook4.Domain;
using TweetBook4.Options;

namespace TweetBook4.Service
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly AppDbContext _dbContext;


        public IdentityService(UserManager<IdentityUser> userManager ,JwtSettings jwtSettings ,TokenValidationParameters validationParameters ,AppDbContext appDbContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = validationParameters;
            _dbContext = appDbContext;

        }

        public async Task<AuthenticatonResult> LoginAsync(string email, string password)
        {
            var User = await _userManager.FindByEmailAsync(email);

            if (User == null)
            {
                return new AuthenticatonResult
                {
                    Errors = new[]
                    {
                        "User do not exists"
                    }
                };
            }

            var UserHasValidPassword = await _userManager.CheckPasswordAsync(User,password);

            if (!UserHasValidPassword)
            {
                return new AuthenticatonResult
                {
                    Errors = new[]
                    {
                        "User/Password combination is wrong"
                    }
                };
            }
            return await GenerateAutheicatoResultForUserAsync(User);
        }

        public async Task<AuthenticatonResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null)
            {
                return new AuthenticatonResult { Errors = new[] { "Invalid Token" } }; 
            }
            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUTC = new DateTime(1970, 1, 1, 0, 0, 0)
                .AddSeconds(expiryDateUnix);
            if(expiryDateTimeUTC > DateTime.Now)
            {
                return new AuthenticatonResult { Errors = new[] { "this token hasn't expired yet" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            // get Stored Token from database
            var storedRefreshToken = await _dbContext.RefreshToken.SingleOrDefaultAsync(x => x.Token == refreshToken);
            if (storedRefreshToken == null)
            {
                return new AuthenticatonResult { Errors = new[] { "this refresh token does not exist" } };
            }

            if (DateTime.Now > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticatonResult { Errors = new[] { "this refresh token has expired" } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticatonResult { Errors = new[] { "this refresh token has been invaldated" } };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticatonResult { Errors = new[] { "this refresh token has been used" } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticatonResult { Errors = new[] { "this refresh token does not match this Jwt" } };
            }

            storedRefreshToken.Used = true;
            _dbContext.RefreshToken.Update(storedRefreshToken);
            await _dbContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateAutheicatoResultForUserAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken)&& jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticatonResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticatonResult
                {
                    Errors = new[]
                    {
                        "User with this email already exists"
                    }
                };
            }

            var newUserId = Guid.NewGuid();

            var newUser = new IdentityUser 
            { 
                Id=newUserId.ToString(),
                Email=email,
                UserName=email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                return new AuthenticatonResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            await _userManager.AddClaimAsync(newUser, new Claim("tags.view", "true"));

            return await GenerateAutheicatoResultForUserAsync(newUser);
              
        }

        private async Task<AuthenticatonResult> GenerateAutheicatoResultForUserAsync(IdentityUser User)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub,User.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,User.Email),
                    new Claim("id",User.Id)
                };

            var userClaims = await _userManager.GetClaimsAsync(User);

            claims.AddRange(userClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),             
                Expires = DateTime.Now.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = User.Id,
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMonths(6)
            };

            await _dbContext.RefreshToken.AddAsync(refreshToken);
                await _dbContext.SaveChangesAsync();

            return new AuthenticatonResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken=refreshToken.Token
            };
        }
    }
}
