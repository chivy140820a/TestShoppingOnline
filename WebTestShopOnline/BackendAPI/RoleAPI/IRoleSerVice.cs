using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.RoleAPI.ModelRoleAPI;
using WebTestShopOnline.BackendAPI.RoleAPI.RoleAssignRequest;

namespace WebTestShopOnline.BackendAPI.RoleAPI
{
   public interface IRoleSerVice
    {
        public Task<List<ModelRole>> GetAllRole();
        public Task<ModelRole> FindRoleByName(string RoleName);
        public Task<bool> CreatRole(CreatRole request);
        public Task<bool> DeleteRole(string RoleName);
        public Task<bool> UpdateRole(UpdateRoleRequest request);
        public Task<bool> RoleAssign(ListRoleItem request);
        public Task<List<string>> FindListNameRole(string tearm);
        public Task<bool> CreatListRole(string ListName);
    }
}
