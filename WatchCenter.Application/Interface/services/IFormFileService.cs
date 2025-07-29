
namespace WatchCenter.Application.Interface.services
{
    public interface IFormFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task<bool> DeleteFileAsync(string imagePath);
    }
}
