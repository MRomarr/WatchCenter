
namespace WatchCenter.Application.Interface.services
{
    public interface ISeriesService
    {
        Task<Result<ICollection<ContentDto>>> GetAllAsync();
        Task<Result<SeriesDto>> GetByIdAsync(string id);
        Task<Result<ICollection<EpisodeDto>>> GetEpsodesAsync(string SeriesId,string SeasonId);
        Task<Result<SeasonDto>> CreateSeasonAsync(string SeriesId);
    }
}
