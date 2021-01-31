using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.SlightAPI.ModelSlightAPI;

namespace WebTestShopOnline.BackendAPI.SlightAPI
{
   public interface ISlightSerVice
    {
        public Task<int> Creat(CreatSlight request); 
    }
}
