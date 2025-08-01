
namespace WatchCenter.Application.Interface.services
{
    public interface IMovieService
    {
        Task<Result<ContentDto>> AddMovieAsync(AddMovieDto dto);
        Task<Result<ICollection<ContentDto>>> GetAllAsync();
        Task<Result<MovieDto>> GetByIdAsync(string id);
    }
}
