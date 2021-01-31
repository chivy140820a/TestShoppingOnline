using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.UserAdminAPI.ModelUserAPI;

namespace WebTestShopOnline.BackendAPI.UserAdminAPI
{
    public class UserAPI : IUserAPI
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        public UserAPI(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;

        }
        public async Task<string> Authentication(AuthenticationRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                throw new Exception("Tk không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),

                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ModelUser> FindByEmail(string Email)
        {
            var username = await _userManager.FindByEmailAsync(Email);
            var roles = await _userManager.GetRolesAsync(username);
            var model = new ModelUser()
            {
                Id = username.Id,
                UserName = username.UserName,
                PhoneNumber = username.PhoneNumber,
                Email = username.Email,
                Roles = roles
            };
            return model;
        }

        public async Task<ModelUser> FindByName(string UserName)
        {
            var username = await _userManager.FindByNameAsync(UserName);
            var roles = await _userManager.GetRolesAsync(username);
            var model = new ModelUser()
            {
                Id = username.Id,
                UserName = username.UserName,
                PhoneNumber = username.PhoneNumber,
                Email = username.Email,
                Roles = roles
            };
            return model;
        }

        public async Task<ModelUser> FindByPhone(string PhoneNumber)
        {
            var username = await _userManager.FindByEmailAsync(PhoneNumber);
            var roles = await _userManager.GetRolesAsync(username);
            var model = new ModelUser()
            {
                Id = username.Id,
                UserName = username.UserName,
                PhoneNumber = username.PhoneNumber,
                Email = username.Email,
                Roles = roles
            };
            return model;
        }

        public async Task<List<string>> FindListNameUser(string term)
        {
            var listname = await _userManager.Users.Where(x => x.UserName.Contains(term)).Select(x => x.UserName).ToListAsync();
            return listname;
        }

        public async Task<List<ModelUser>> GetAllUser()
        {
            var listUser = await _userManager.Users.ToListAsync();
            var getall = listUser.Select(x => new ModelUser()
            {
                Id = x.Id,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
            }).ToList();
            return getall;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = new IdentityUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };
            var creat = await _userManager.CreateAsync(user, request.Password);
            if (creat.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
