using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.Data;
using WebTestShopOnline.Entity;
using WebTestShopOnline.Web.ConnectAPI.UserAPICN;

namespace WebTestShopOnline.Web.Controllers
{
    public class CommonController : Controller
    {
        private readonly IUserConnectAPI _userConnectAPI;
        private readonly ApplicationDbContext _context;
        public CommonController(ApplicationDbContext context, IUserConnectAPI userConnectAPI)
        {
            _userConnectAPI = userConnectAPI;
            _context = context;
        }
        public async Task<IActionResult> FindUserByName()
        {
            var username = User.Identity.Name;
            var user = await _userConnectAPI.FindByName(username);
            return Json(new
            {
                status = true,
                data = user
            });
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult UserLogin()
        {
            var user = User.Identity.Name;
            return Json(new
            {
                status = true,
                data = user
            });
        }
        
        public JsonResult GetAllProductCategory()
        {
            var listproductcategory = _context.ProductCategories.ToList();
            return Json(new
            {
                status = true,
                data = listproductcategory
            });
        }
        public JsonResult GetAllProduct(string term)
        {
            var listproduct = _context.Products.Where(x => x.Name.Contains(term)).Select(x => x.Name).ToList();
            return Json(new
            {
                status = true,
                data = listproduct
            });
        }
        public JsonResult GetAllBody()
        {
            var listbody = _context.Bodies.ToList();
            return Json(new
            {
                status = true,
                data = listbody
            });
        }
        public JsonResult AddContact(string Name,string Email,string SubJect,string Status)
        {
            var contact = new Content();
            contact.Name = Name;
            contact.Email = Email;
            contact.SubJect = SubJect;
            contact.Status = Status;
            _context.Contents.Add(contact);
            _context.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
        public IActionResult Delivery()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Success()
        {
            return View();
        }
    }
}
