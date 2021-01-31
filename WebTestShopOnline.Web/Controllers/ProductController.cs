using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.Data;
using WebTestShopOnline.Entity;
using WebTestShopOnline.Page;

namespace WebTestShopOnline.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> GetAllProduct(string keyword,int categoryId,int pageIndex=1,int pageSize=9)
        {
            var listproductcategory = await _context.ProductCategories.ToListAsync();
            ViewBag.ListProductCategory = listproductcategory.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var request = new PageRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword,
                CategoryId=categoryId
            };
            string productcategory = null;
            if (request.CategoryId > 0)
            {
                var name = await _context.ProductCategories.FindAsync(categoryId);
                productcategory = name.Name;
            }

            var listproduct = await _context.Products.ToListAsync();
            var product = from p in listproduct
                          select new { p };
            if(!string.IsNullOrEmpty(request.Keyword)&& request.CategoryId > 0)
            {
                product = product.Where(x => x.p.Name.Contains(request.Keyword));
                product = product.Where(x => x.p.ProductCategoryId == request.CategoryId);
            }
            else
            {
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    product = product.Where(x => x.p.Name.Contains(request.Keyword));
                }
                if (request.CategoryId ==1 || request.CategoryId==2)
                {
                    product = product.Where(x => x.p.ProductCategoryId == request.CategoryId);
                }
                if (request.CategoryId == 3)
                {
                    product = product.ToList();
                }
            }
            var total = product.Count();
            var vt = product.Skip((request.PageIndex - 1) * (request.PageSize)).Take(request.PageSize)
                .Select(x => new Product()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    PathImage = x.p.PathImage,
                    Price = x.p.Price,
                    LastPrice = x.p.LastPrice
                }).ToList();
            var page = new PagedResult<Product>()
            {
                Items = vt,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = total,
                
            };
            if (request.CategoryId > 0)
            {
                page.NameCategory = productcategory;
            }
            return View(page);
            
        }
        [Route("Product/ProductDetail/{Id}")]
        public async Task<IActionResult> ProductDetail(int Id)
        {
            var product  = await _context.Products.FindAsync(Id);
            var productImage = await _context.Images.Where(x => x.ProductId == Id).Take(3).ToListAsync();
            ViewBag.ListProductImage = productImage;
            return View(product);
        }
        public JsonResult ListProductCategory()
        {
            var listproductcategory = _context.ProductCategories.ToList();
            return Json(new
            {
                status = true,
                data = listproductcategory
            });
        }
        public JsonResult ListColor()
        {
            var listcolor = _context.Colors.ToList();
            return Json(new
            {
                status = true,
                data= listcolor
            });
        }
    }
}
