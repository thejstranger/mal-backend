using AutoMapper;
using MAL.DAL;
using MAL.DTO;
using MAL.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using MAL.Common;

namespace MAL.Services
{
    public class UserService : BaseService, IUserService
    {
        public readonly IMapper _mapper;
        public UserService(IDatabaseContext madContext, IMapper mapper) : base(madContext)
        {
            _mapper = mapper;
        }
        public List<UserDto> GetAllUsers()
        {
            var users = _dbContext.UserRepository.Get();
            return _mapper.Map<List<UserDto>>(users);
        }
        public string LoginUser(UserDto UserDto)
        {
            var hashedPassword = UserDto.Password;

            var user = _dbContext.UserRepository.GetSingle( x => x.Username == UserDto.Username && x.Password == hashedPassword);

            return BuildToken(user);
        }

        private string BuildToken(Users user)
        {
            if (user == null)
            {
                return "";
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var config = new ConfigProvider();
            var issuer = config.GetJwtIssuer();
            var configkey = config.GetJwtKey();
            var aud = config.GetJwtAudience();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: aud,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
