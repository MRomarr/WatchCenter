
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WatchCenter.Infrasturcture.Services.FormFileService
{
    internal class FormFileService : IFormFileService
    {
        private readonly IWebHostEnvironment _env;

        public FormFileService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var uploadsFolder = Path.Combine(_env.WebRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{folderName}/{fileName}";
        }

        public async Task<bool> DeleteFileAsync(string imagePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, imagePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }

    }
}
