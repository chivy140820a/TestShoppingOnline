using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.RoleAPI.ModelRoleAPI;
using WebTestShopOnline.BackendAPI.RoleAPI.RoleAssignRequest;

namespace WebTestShopOnline.Web.ConnectAPI.RoleAPICN
{
    public class RoleConnectAPI : IRoleConnectAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RoleConnectAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public  async Task<bool> CreatListRole(string ListName)
        {
            var json = JsonConvert.SerializeObject(ListName);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/RoleAdmin/CreatListRole", jsonstring);
            return post.IsSuccessStatusCode;
        }

        public async Task<bool> CreatRole(CreatRole request)
        {
            var json = JsonConvert.SerializeObject(request);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/RoleAdmin/Creat", jsonstring);
            return post.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRole(string Name)
        {
            var json = JsonConvert.SerializeObject(Name);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/RoleAdmin/Delete", jsonstring);
            return post.IsSuccessStatusCode;
        }

        public async Task<ModelRole> FindByName(string Name)
        {
            var json = JsonConvert.SerializeObject(Name);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/RoleAdmin/FindRoleByName",jsonstring);
            var readpost = await post.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ModelRole>(readpost);
            return product;
        }

        public async Task<List<string>> FindListNameRole(string term)
        {
            var json = JsonConvert.SerializeObject(term);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/RoleAdmin/FindListNameRole", jsonstring);
            var readpost = await post.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<List<string>>(readpost);
            return product;
        }

        public async Task<List<ModelRole>> GetAll()
        {
           
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.GetAsync("api/RoleAdmin/GetAll");
            var readpost = await post.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<List<ModelRole>>(readpost);
            return product;
        }

        public async Task<bool> RoleAssignRequest(ListRoleItem request)
        { 
            var json = JsonConvert.SerializeObject(request);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/RoleAdmin/RoleAssign", jsonstring);
            return post.IsSuccessStatusCode;
        }

        public async Task<bool> Update(UpdateRoleRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var jsonstring = new StringContent(json, Encoding.UTF8, "application/json");
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.PostAsync("api/RoleAdmin/Update", jsonstring);
            return post.IsSuccessStatusCode;
        }
    }
}
