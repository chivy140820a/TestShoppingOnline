using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.Data;
using WebTestShopOnline.Web.Models;

namespace WebTestShopOnline.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
           
            // Lấy ra tất cả slight 
            var listslight = _context.Slights.ToList();
            ViewBag.ListSight = listslight;
            // Lấy Slight gốc
            ViewBag.SlightAdmin = _context.Slights.Find(2);
            //Lấy List máy ảnh
            ViewBag.ListMA = _context.Products.Where(x => x.ProductId == 1).Take(4).ToList();
            // Lất List thẻ nhớ
            ViewBag.ListTN = _context.Products.Where(x => x.ProductId == 2).Take(4).ToList();
            //Lấy List giày
            ViewBag.ListG = _context.Products.Where(x => x.ProductId == 3).Take(4).ToList();
            // Lấy Laters Product
            ViewBag.ListLastProduct = _context.Products.Take(6).ToList();
            var user = User.Identity.Name;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult OnGetSetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
