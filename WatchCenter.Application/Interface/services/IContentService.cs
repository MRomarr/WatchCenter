
namespace WatchCenter.Application.Interface.services
{
    public interface IContentService
    {
        Task<Result<ContentDto>> GetByIdAsync(string id);
        Task<Result<IEnumerable<ContentDto>>> SearchAsync(string query,int Page,int PageSize);
        Task<Result<ContentDto>> CreateAsync(string UserId, CreateContentDto dto);
        Task<Result<ContentDto>> UpdateAsync(UpdateContentDto dto);
        Task<Result> DeleteAsync(string id);
    }
}
