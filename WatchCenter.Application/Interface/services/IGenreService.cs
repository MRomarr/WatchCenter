
namespace WatchCenter.Application.Interface.services
{
    public interface IGenreService
    {
        Task<Result<IEnumerable<GenreDto>>> GetAllGenresAsync();
        Task<Result<IEnumerable<ContentDto>>> GetContentsByGenreIdAsync(string id);
        Task<Result<GenreDto>> CreateGenreAsync(string GenreName);
        Task<Result<GenreDto>> UpdateGenreAsync(UpdateGenreDto dto);
        Task<Result> DeleteGenreAsync(string id);
    }
}
