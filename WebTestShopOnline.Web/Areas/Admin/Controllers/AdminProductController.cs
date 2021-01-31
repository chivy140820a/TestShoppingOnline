using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.ProductAPI.ModelProductAPI;
using WebTestShopOnline.Data;
using WebTestShopOnline.Entity;
using WebTestShopOnline.Page;
using WebTestShopOnline.Web.ConnectAPI.ProductAPICN;

namespace WebTestShopOnline.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductConnectAPI _productConnectAPI;
        public AdminProductController(IProductConnectAPI productConnectAPI,ApplicationDbContext context)
        {
            _context = context;
            _productConnectAPI = productConnectAPI;
        }
       
        [HttpGet]
        public async Task<IActionResult> RemoveListProduct()
        {
            var listproduct = await _context.Products.ToListAsync();
            var ListProductItem = new ListProductItem();
            foreach(var product in listproduct)
            {
                ListProductItem.Items.Add(new ProductItem()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Selected = false,
                    PathImage = product.PathImage
                });
            }
            return View(ListProductItem);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveListProduct(ListProductItem request)
        {
            var list = request.Items.Where(x => x.Selected == true).ToList();
            foreach(var product in list)
            {
                var prop = await _context.Products.FindAsync(product.Id);
                _context.Products.Remove(prop);

            }
            await _context.SaveChangesAsync();
            TempData["result"] = "Xóa thành công";
            return RedirectToAction("GetAllProduct", "AdminProduct", "Admin");

        }
        [HttpGet]
        [Route("AdminProduct/ProductAssign/{Id}")]
        public async Task<IActionResult> ProductAssign(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            var listproductcategory = await _context.ProductCategories.Where(x => x.ProductId == product.ProductCategoryId).Select(x => x.Name).ToListAsync();
            var listproduct = await _context.ProductCategories.ToListAsync();
            var listproductItem = new ListProductItem();
            listproductItem.Id = Id;
            foreach(var prop in listproduct)
            {
                listproductItem.Items.Add(new ProductItem()
                {
                    Id = prop.Id,
                    Name = prop.Name,
                    Selected = listproductcategory.Contains(prop.Name)
                });
            }
            return View(listproductItem);
        }
        [HttpPost]
        [Route("AdminProduct/ProductAssign/{Id}")]
        public  async Task<IActionResult> ProductAssign(ListProductItem request)
        {
            var productadmin = await _context.Products.FindAsync(request.Id);
            var removeList = request.Items.Where(x => x.Selected == false).ToList();
            foreach(var product in removeList)
            {
                productadmin.ProductCategoryId = 0;
                _context.Products.Update(productadmin);

            }
            var addList = request.Items.Where(x => x.Selected == true).ToList();
            foreach(var productadd in addList)
            {
                var category = await _context.ProductCategories.FindAsync(productadd.Id);
                productadmin.ProductCategoryId = category.ProductId;
                _context.Products.Update(productadmin);
            }
             await _context.SaveChangesAsync();
            return RedirectToAction("GetAllProduct", "AdminProduct", "Admin");
        }
        public async Task<IActionResult> GetAllProduct(string keyword,int categoryId, int pageIndex=1,int pageSize=5)
        
        {
            
            var request = new PageRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword,
                CategoryId= categoryId
            };
            var listproduct = await _productConnectAPI.GetAllProduct();
            var product = from p in listproduct
                          select new { p };
            if (!string.IsNullOrEmpty(request.Keyword) && request.CategoryId > 0)
            {
                product = product.Where(x => x.p.Name.Contains(request.Keyword));
                product = product.Where(x => x.p.ProductId == request.CategoryId);
            }
            else
            {
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    product = product.Where(x => x.p.Name.Contains(request.Keyword));
                }
                if (request.CategoryId > 0)
                {
                    product = product.Where(x => x.p.ProductId == request.CategoryId);
                }
            }
            
            
            var total = product.Count();
            var vt = product.Skip((request.PageIndex - 1) * (request.PageSize)).Take(request.PageSize)
                .Select(x => new Product()
                {
                    Id = x.p.Id,
                    Name=x.p.Name,
                    Price=x.p.Price,
                    LastPrice=x.p.LastPrice,
                    PathImage=x.p.PathImage
                }).ToList();
            var page = new PagedResult<Product>()
            {
                TotalRecords = total,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = vt
            };
            if(TempData["result"] != null)
            {
                ViewBag.Message = TempData["result"];
            }
            return View(page);

        }
        public JsonResult FindProductById(int Id)
        {
            var product = _context.Products.Find(Id);
            return Json(new
            {
                status = true,
                data = product
            });
        }

        [HttpGet]
        [Route("/Admin/ProductUpdate/{Id}")]
        public async Task<IActionResult> Update(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            var update = new UpdateProductAPI()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                LastPrice = product.LastPrice,

            };
            return View(update);
        }

        [HttpPost]
        [Route("/Admin/ProductUpdate/{Id}")]
        public async Task<IActionResult> Update(UpdateProductAPI request)
        {
            var find = await _productConnectAPI.UpdateProduct(request);
            if (find == true)
            {
                TempData["result"] = "Cập nhập thành công";
                return RedirectToAction("GetAllProduct", "AdminProduct");
            }
            else
            {
                TempData["result"] = "Cập nhập không thành công";
                return RedirectToAction("GetAllProduct", "AdminProduct");
            }
        }
        public JsonResult DeleteProduct(int Id)
        {

            var product = _context.Products.Find(Id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        [HttpGet]
        public IActionResult CreatProduct()
        {
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> CreatProduct(CreatProductAPI request)
        {
            var product = await _productConnectAPI.CreatProduct(request);
            if (product == true)
            {
                TempData["result"] = "Tạo thành công";
                return RedirectToAction("GetAllProduct", "AdminProduct");
            }
            else
            {
                TempData["result"] = "Tạo thành công";
                return RedirectToAction("GetAllProduct", "AdminProduct");
            }
        }
        public JsonResult FindListNameProduct(string term)
        {
            var listnameFindProduct = _context.Products.Where(x => x.Name.Contains(term)).Select(x => x.Name).ToList();
            return Json(new
            {
                status = true,
                data = listnameFindProduct
            });
        }
        public JsonResult ListProductCategoryId()
        {
            var listproductcategory = _context.ProductCategories.ToList();
            return Json(new
            {
                status = true,
                data = listproductcategory
            });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
