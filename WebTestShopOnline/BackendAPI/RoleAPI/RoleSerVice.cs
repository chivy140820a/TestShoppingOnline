using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.RoleAPI.ModelRoleAPI;
using WebTestShopOnline.BackendAPI.RoleAPI.RoleAssignRequest;

namespace WebTestShopOnline.BackendAPI.RoleAPI
{
    public class RoleSerVice : IRoleSerVice
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public RoleSerVice(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }

        public  async Task<bool> CreatListRole(string ListName)
        {
            var listname = JsonConvert.DeserializeObject<List<ModelCreatListRole>>(ListName);
            foreach(var rolename in listname)
            {
                var role = new IdentityRole()
                {
                    Name = rolename.Name
                };
                await _roleManager.CreateAsync(role);
            }
            return true;
        }

        public async Task<bool> CreatRole(CreatRole request)
        {
            var role = new IdentityRole()
            {
                Name = request.Name
            };
            var creat = await _roleManager.CreateAsync(role);
            if(creat.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteRole(string RoleName)
        {
            var role = await _roleManager.FindByNameAsync(RoleName);
            var deleteRole = await _roleManager.DeleteAsync(role);
            if(deleteRole.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<string>> FindListNameRole(string tearm)
        {
            var listnameRole = await _roleManager.Roles.Where(x => x.Name.Contains(tearm)).Select(x => x.Name).ToListAsync();
            return listnameRole;
        }

        public async Task<ModelRole> FindRoleByName(string RoleName)
        {
            var role = await _roleManager.FindByNameAsync(RoleName);
            var model = new ModelRole()
            {
                Id = role.Id,
                Name = role.Name
            };
            return model;
        }

        public async Task<List<ModelRole>> GetAllRole()
        {
            var role = await _roleManager.Roles.Select(x => new ModelRole()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return role;
        }

        public async Task<bool> RoleAssign(ListRoleItem request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var listremoveRole = request.Items.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            var listaddRole = request.Items.Where(x => x.Selected == true).Select(x => x.Name).ToList();
            foreach(var remove in listremoveRole)
            {
                if( await _userManager.IsInRoleAsync(user,remove)==true)
                {
                   await  _userManager.RemoveFromRoleAsync(user, remove);
                }    

            }
            foreach (var add in listaddRole)
            {
                if (await _userManager.IsInRoleAsync(user, add) == true)
                {
                    await _userManager.AddToRoleAsync(user, add);
                }

            }
            return true;
        }

        public async Task<bool> UpdateRole(UpdateRoleRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.Name);
            role.Name = request.NewName;
            var update = await _roleManager.UpdateAsync(role);
            if(update.Succeeded)
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
