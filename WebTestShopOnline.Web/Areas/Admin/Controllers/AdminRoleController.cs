using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.RoleAPI.ModelRoleAPI;
using WebTestShopOnline.BackendAPI.RoleAPI.RoleAssignRequest;
using WebTestShopOnline.Page;
using WebTestShopOnline.Web.ConnectAPI.RoleAPICN;
using WebTestShopOnline.Web.ConnectAPI.UserAPICN;

namespace WebTestShopOnline.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminRoleController : Controller
    {
        private readonly IUserConnectAPI _userConnectAPI;
        private readonly IRoleConnectAPI _context;
        public AdminRoleController(IRoleConnectAPI context, IUserConnectAPI userConnectAPI)
        {
            _userConnectAPI = userConnectAPI;
            _context = context;
        }
        public async Task<IActionResult> GetAllRole(string keyword,int pageIndex=1,int pageSize=5)
        {
            var request = new PageRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };
            var listroleitem = await _context.GetAll();
            var product = from p in listroleitem
                          select new { p };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                product = product.Where(x => x.p.Name.Contains(request.Keyword));
            }
            var total = product.Count();
            var vt = product.Skip((request.PageIndex - 1) * (request.PageSize)).Take(request.PageSize)
                .Select(x => new ModelRole()
                {
                    Id = x.p.Id,
                    Name=x.p.Name
                }).ToList();
            var page = new PagedResult<ModelRole>()
            {
                TotalRecords = total,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = vt
            };
            return View(page);
            
            
        }
        [HttpGet]
        [Route("/Role/RoleAssign/{UserName}")]
        public async Task<IActionResult> RoleAssign(string UserName)
        {
            var user = await  _userConnectAPI.FindByName(UserName);
            var listRoleItem = new ListRoleItem();
            listRoleItem.UserName = UserName;
            var getall = await _context.GetAll();
            foreach(var role in getall)
            {
                listRoleItem.Items.Add(new RoleItem()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Selected= user.Roles.Contains(role.Name)
                });
            } 
            
            return View(listRoleItem);
        }
        [HttpPost]
        [Route("/Role/RoleAssign/{UserName}")]
        public async Task<IActionResult> RoleAssign(ListRoleItem request)
        {
            var find = await _context.RoleAssignRequest(request);
            if (find == true)
            {
                TempData["result"] = "Phân quyền thành công";
                return RedirectToAction("GetAllRole", "AdminRole");
            }
            else
            {
                TempData["result"] = "Phân quyền không thành công";
                return RedirectToAction("GetAllRole", "AdminRole");
            }
        }
        public async Task<IActionResult> FindListRoleName(string term)
        {
            var find = await _context.FindListNameRole(term);
            return Json(new
            {
                status = true,
                data = find
            });
        }
        public async Task<IActionResult> FindRoleByName(string RoleName)
        {
            var role = await _context.FindByName(RoleName);
            return Json(new
            {
                status = true,
                data = role
            });
        }
        public async Task<IActionResult> UpdateRole(UpdateRoleRequest request)
        {
            var update = await _context.Update(request);
            return Json(new
            {
                status = true,

            });
        }
        public async Task<IActionResult> DeleteRole(string RoleName)
        {
            var delete = await _context.DeleteRole(RoleName);
            return Json(new
            {

                status = true,

            });
        }
        public async Task<IActionResult> CreatListRole(string ListName)
        {
            var find = await _context.CreatListRole(ListName);
            return Json(new
            {
                status = true
            });
        }
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
