using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTestShopOnline.BackendAPI.UserAdminAPI.ModelUserAPI
{
    public class ModelUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
