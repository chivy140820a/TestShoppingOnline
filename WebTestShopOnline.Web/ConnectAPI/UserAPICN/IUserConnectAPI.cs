using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.UserAdminAPI.ModelUserAPI;

namespace WebTestShopOnline.Web.ConnectAPI.UserAPICN
{
   public interface IUserConnectAPI
    {
        public Task<string> Authentication(AuthenticationRequest request);
        public Task<bool> Register(RegisterRequest request);
        public Task<List<ModelUser>> GetAllUser();
        public Task<ModelUser> FindByName(string UserName);
        public Task<ModelUser> FindByPhone(string PhoneNumber);
        public Task<ModelUser> FindByEmail(string Email);
        public Task<List<string>> FindListNameUser(string tearm);
    }
}
