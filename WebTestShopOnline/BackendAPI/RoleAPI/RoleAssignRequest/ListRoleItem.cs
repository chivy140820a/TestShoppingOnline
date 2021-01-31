using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTestShopOnline.BackendAPI.RoleAPI.RoleAssignRequest
{
    public class ListRoleItem
    {
        public string UserName { get; set; }
        public List<RoleItem> Items { get; set; } = new List<RoleItem>();
    }
}
