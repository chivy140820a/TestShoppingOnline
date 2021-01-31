using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTestShopOnline.BackendAPI.RoleAPI.ModelRoleAPI
{
    public class UpdateRoleRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NewName { get; set; }
    }
}
