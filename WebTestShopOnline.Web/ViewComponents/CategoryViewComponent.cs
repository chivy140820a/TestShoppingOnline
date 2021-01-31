using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.Data;

namespace WebTestShopOnline.Web.ViewComponents
{
    public class CategoryViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CategoryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Id=1)
        {
            var product = await _context.Products.FindAsync(Id);
            ViewBag.ListCategoryElectronic = await _context.Products.Where(x => x.ProductId == 1).ToListAsync();
            ViewBag.ListCategoryClother = await _context.Products.Where(x => x.ProductId == 2).ToListAsync();
            ViewBag.ImageAdmin = await _context.Products.Where(x => x.ProductId == 1).Take(2).ToListAsync();
            return View();
        }
    }
}
