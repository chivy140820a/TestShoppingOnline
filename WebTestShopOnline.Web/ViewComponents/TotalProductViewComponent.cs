using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.CartToBuy;
using WebTestShopOnline.Data;

namespace WebTestShopOnline.Web.ViewComponents
{
    public class TotalProductViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public TotalProductViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            var getall = HttpContext.Session.GetString("CartRequest");
            
            var listcartItem = JsonConvert.DeserializeObject<List<CartItem>>(getall);
            var total = new TotalProduct();
            total.TotalPrice = 0;
            if (listcartItem.Exists(x => x.Product.Id == Id))
            {
                foreach(var cart in listcartItem)
                {
                    if (cart.Product.Id == Id)
                    {
                        total.TotalPrice = (cart.Quantity) * (cart.Product.Price);
                    }
                }
            }
            return View(total);
        }
    }
}
