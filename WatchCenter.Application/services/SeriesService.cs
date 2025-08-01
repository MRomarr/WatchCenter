namespace WatchCenter.Application.services
{
    internal class SeriesService : ISeriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SeriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<ContentDto>>> GetAllAsync()
        {
            var result = await _unitOfWork.Contents.GetAllAsync();
            result = result.Where(c => (int)c.Type == 1);
            var contentdto = _mapper.Map<ICollection<ContentDto>>(result);

            return Result<ICollection<ContentDto>>.Success(contentdto);
        }

        public async Task<Result<SeriesDto>> GetByIdAsync(string id)
        {
            var series = await _unitOfWork.Contents.GetByIdAsync(id, m => m.Series,m=>m.Series.Seasons);

            // check if series null
            if (series is null)
                return Result<SeriesDto>.Failure("Series not found");

            var seriesDto = _mapper.Map<SeriesDto>(series);

            return Result<SeriesDto>.Success(seriesDto);
        }

        public async Task<Result<ICollection<EpisodeDto>>> GetEpsodesAsync(string SeriesId, string SeasonId)
        {
            var content = await _unitOfWork.Contents.GetByIdAsync(SeriesId,s=>s.Series.Seasons);
            if (content is null)
                return Result<ICollection<EpisodeDto>>.Failure("series not found");

            var seasons = content.Series.Seasons;
            var season = seasons.FirstOrDefault(s=>s.Id==SeasonId);
            if (season is null)
                return Result<ICollection<EpisodeDto>>.Failure("season not found");

            var epsodes = season.Episodes;
            var epsodesDto = _mapper.Map< ICollection<EpisodeDto>>(epsodes);

            return Result<ICollection<EpisodeDto>>.Success(epsodesDto);

        }
        public async Task<Result<SeasonDto>> CreateSeasonAsync(string SeriesId)
        {
            var series = await _unitOfWork.Contents.GetByIdAsync(SeriesId,s=>s.Series);
            if (series is null)
                return Result<SeasonDto>.Failure("not found");

            var seasons = await _unitOfWork.seasons.GetAllAsync();
            var seriesseasons = seasons.Where(s=>s.SeriesId==SeriesId);
            var lastseason = seriesseasons.OrderByDescending(s=>s.SeasonNumber).FirstOrDefault();
            
            var season = new Season()
            {
                SeriesId = series.Series.Id,
                SeasonNumber = lastseason is not null? lastseason.SeasonNumber+1 : 1
            };
            await _unitOfWork.seasons.AddAsync(season);
            var result = await _unitOfWork.SaveAsync();
            if(!result)
                return Result<SeasonDto>.Failure("feild to save");
            var data = _mapper.Map<SeasonDto>(season);
            return Result<SeasonDto>.Success(data);

        }
    }
}
