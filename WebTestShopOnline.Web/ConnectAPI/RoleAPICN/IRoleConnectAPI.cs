using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.RoleAPI.ModelRoleAPI;
using WebTestShopOnline.BackendAPI.RoleAPI.RoleAssignRequest;

namespace WebTestShopOnline.Web.ConnectAPI.RoleAPICN
{
   public interface IRoleConnectAPI
    {
        public Task<List<ModelRole>> GetAll();
        public Task<ModelRole> FindByName(string Name);
        public Task<bool> DeleteRole(string Name);
        public Task<bool> Update(UpdateRoleRequest request);
        public Task<bool> CreatRole(CreatRole request);
        public Task<bool> RoleAssignRequest(ListRoleItem request);
        public Task<List<string>> FindListNameRole(string term);
        public Task<bool> CreatListRole(string ListName);
    }
}
