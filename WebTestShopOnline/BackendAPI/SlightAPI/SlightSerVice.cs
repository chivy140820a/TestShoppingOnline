using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebTestShopOnline.BackendAPI.SlightAPI.ModelSlightAPI;
using WebTestShopOnline.Data;
using WebTestShopOnline.Entity;
using WebTestShopOnline.ImageManager;

namespace WebTestShopOnline.BackendAPI.SlightAPI
{
    public class SlightSerVice : ISlightSerVice
    {
        private readonly IStorageService _storageService;
        private readonly ApplicationDbContext _context;
        public SlightSerVice(ApplicationDbContext context, IStorageService storageService)
        {
            _storageService = storageService;
            _context = context;
        }
        public async Task<int> Creat(CreatSlight request)
        {
            var slght = new Slight()
            {
                Name = request.Name,
                PathImage = await this.SaveFile(request.ThumImage)
            };
            _context.Slights.Add(slght);
            return await  _context.SaveChangesAsync();
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
