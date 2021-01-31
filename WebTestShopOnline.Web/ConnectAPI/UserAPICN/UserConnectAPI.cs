using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.UserAdminAPI.ModelUserAPI;

namespace WebTestShopOnline.Web.ConnectAPI.UserAPICN
{
    public class UserConnectAPI : IUserConnectAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserConnectAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> Authentication(AuthenticationRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/UserAdmin/Authentication", jsonstring);
            var readpost = await post.Content.ReadAsStringAsync();
            return readpost;
        }

        public async Task<ModelUser> FindByEmail(string Email)
        {
            var json = JsonConvert.SerializeObject(Email);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/UserAdmin/FindByEmail", jsonstring);
            var readpost = await post.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ModelUser>(readpost);
            return product;
        }

        public async Task<ModelUser> FindByName(string UserName)
        {
            var json = JsonConvert.SerializeObject(UserName);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/UserAdmin/FindByName", jsonstring);
            var readpost = await post.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ModelUser>(readpost);
            return product;
        }

        public async Task<ModelUser> FindByPhone(string PhoneNumber)
        {
            var json = JsonConvert.SerializeObject(PhoneNumber);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/UserAdmin/FindByPhone", jsonstring);
            var readpost = await post.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ModelUser>(readpost);
            return product;
        }

        public async Task<List<string>> FindListNameUser(string tearm)
        {
            var json = JsonConvert.SerializeObject(tearm);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/UserAdmin/FindListNameUser", jsonstring);
            var readpost = await post.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<List<string>>(readpost);
            return product;
        }

        public async Task<List<ModelUser>> GetAllUser()
        {
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.GetAsync("api/UserAdmin/GetAllUser");
            var readpost = await post.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<List<ModelUser>>(readpost);
            return product;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/UserAdmin/Register", jsonstring);
            return post.IsSuccessStatusCode;
        }
    }
}
