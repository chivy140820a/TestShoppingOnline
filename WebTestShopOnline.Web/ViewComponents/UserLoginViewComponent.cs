using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.Data;

namespace WebTestShopOnline.Web.ViewComponents
{
    public class UserLoginViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public UserLoginViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            var user = User.Identity.Name;
            ViewBag.User = user;
            return View();
        }
    }
}
