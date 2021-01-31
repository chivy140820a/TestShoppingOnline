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
    public class CartViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CartViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            var total = new TotalProductInCart();
            var product = await _context.Products.FindAsync(Id);
            var getall = HttpContext.Session.GetString("CartRequest");
            if (getall == null)
            {
                total.Total = 0;
            }
            else
            {
                var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);
                total.Total = listcart.Count();
            }

            return View(total);
        }
    }
}
