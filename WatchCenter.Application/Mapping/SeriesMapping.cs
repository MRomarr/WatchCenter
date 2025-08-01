
namespace WatchCenter.Application.Mapping
{
    public class SeriesMapping : Profile
    {
        public SeriesMapping()
        {
            CreateMap<Content, SeriesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.TrailerUrl, opt => opt.MapFrom(src => src.TrailerUrl))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type))
                .ForMember(dest => dest.Seasons, opt => opt.MapFrom(src => src.Series != null ? src.Series.Seasons : null));

            CreateMap<Episode, EpisodeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EpisodeNumber, opt => opt.MapFrom(src => src.EpisodeNumber))
                .ForMember(dest => dest.EpisodeUrl, opt => opt.MapFrom(src => src.EpisodeUrl));
            CreateMap<Season, SeasonDto>();


        }
    }
}
