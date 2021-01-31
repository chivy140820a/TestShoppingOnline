using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.ProductAPI.ModelProductAPI;
using WebTestShopOnline.Entity;

namespace WebTestShopOnline.Web.ConnectAPI.ProductAPICN
{
    public class ProductConnectAPI : IProductConnectAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductConnectAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreatProduct(CreatProductAPI request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5000");


            var requestContent = new MultipartFormDataContent();

            if (request.ThumImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumImage", request.ThumImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.LastPrice.ToString()), "lastPrice");


            var response = await client.PostAsync($"/api/ProductAdmin/CreatProduct", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Product>> GetAllProduct()
        {
            var creat = _httpClientFactory.CreateClient();
            creat.BaseAddress = new Uri("http://localhost:5000");
            var post = await creat.GetAsync("api/ProductAdmin/GetAllProduct");
            var readpost = await post.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<List<Product>>(readpost);
            return product;
        }

        public async Task<bool> UpdateProduct(UpdateProductAPI request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5000");


            var requestContent = new MultipartFormDataContent();

            if (request.ThumImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumImage", request.ThumImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Id.ToString()), "id");
            requestContent.Add(new StringContent(request.LastPrice.ToString()), "lastPrice");


            var response = await client.PostAsync($"/api/ProductAdmin/UpdateProduct", requestContent);
            return response.IsSuccessStatusCode;
        }
    }
}
