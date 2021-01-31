using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.ProductAPI.ModelProductAPI;
using WebTestShopOnline.Entity;

namespace WebTestShopOnline.Web.ConnectAPI.ProductAPICN
{
   public interface IProductConnectAPI
    {
        public Task<List<Product>> GetAllProduct();
        public Task<bool> CreatProduct(CreatProductAPI request);
        public Task<bool> UpdateProduct(UpdateProductAPI request);
    }
}
