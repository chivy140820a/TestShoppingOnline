using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.UserAdminAPI.ModelUserAPI;
using WebTestShopOnline.Page;
using WebTestShopOnline.Web.ConnectAPI.UserAPICN;

namespace WebTestShopOnline.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminUserController : Controller
    {

        private readonly IUserConnectAPI _context;
        public AdminUserController(IUserConnectAPI context)
        {
            _context = context;
        }
         
        public async Task<IActionResult> GetAllUser(string keyword,int pageIndex=1,int pageSize=5)
        {
            
            var request = new PageRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };
            var listuser = await _context.GetAllUser();
            var product = from p in listuser
                          select new { p };
            if(!string.IsNullOrEmpty(request.Keyword))
            {
                product = product.Where(x => x.p.UserName.Contains(request.Keyword));
            }
            var total = product.Count();
            var vt = product.Skip((request.PageIndex - 1) * (request.PageSize)).Take(request.PageSize)
                .Select(x => new ModelUser()
                {
                    Id = x.p.Id,
                    UserName = x.p.UserName,
                    PhoneNumber = x.p.PhoneNumber,
                    Email = x.p.Email
                }).ToList();
            var page = new PagedResult<ModelUser>()
            {
                TotalRecords = total,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = vt
            };
            return View(page);
        }
        public async Task<IActionResult> GetListNameUser(string term)
        {
            var listnameproduct = await _context.FindListNameUser(term);
            return Json(new
            {
                status = true,
                data = listnameproduct
            });
              
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
