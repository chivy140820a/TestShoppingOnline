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
    public class TotalPriceCartViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public TotalPriceCartViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            var total = new TotalProduct();
            total.TotalPrice = 0;
            var getall = HttpContext.Session.GetString("CartRequest");
            if (getall == null)
            {
                total.TotalPrice = 0;
            }
            else
            {
                var listcartItem = JsonConvert.DeserializeObject<List<CartItem>>(getall);
                foreach (var cart in listcartItem)
                {
                    total.TotalPrice += total.TotalPrice + (cart.Product.Price) * (cart.Quantity);
                }
            }
         
            return View(total);
        }
    }
}
