using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTestShopOnline.BackendAPI.SlightAPI.ModelSlightAPI
{
    public class CreatSlight
    {
        public string Name { get; set; }
        public IFormFile ThumImage { get; set; }
    }
}
