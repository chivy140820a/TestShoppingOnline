using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.Data;

namespace WebTestShopOnline.Web.Controllers
{
    public class SlightController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SlightController(ApplicationDbContext context)
        {
            _context = context;
        }
        public JsonResult ListColor()
        {
            var listcolor = _context.Colors.ToList();
            return Json(new
            {
                status = true,
                data=listcolor
            });
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Slight/SlightDetail/{Id}")]
        public async Task<IActionResult> SlightDetail(int Id)
        {
            var slight = await _context.Slights.FindAsync(Id);
            var listproduct = await _context.Images.Take(3).ToListAsync();
            ViewBag.ListProductImage = listproduct;
            return View(slight);
        }
    }
}
