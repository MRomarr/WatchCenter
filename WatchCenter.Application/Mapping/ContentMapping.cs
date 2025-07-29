
using AutoMapper;

namespace WatchCenter.Application.Mapping
{
    internal class ContentMapping : Profile
    {
        public ContentMapping()
        {
            CreateMap<Content, ContentDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type));
            CreateMap<Movie, MovieDto>();
             
            CreateMap<Series, SeriesDto>();
            CreateMap<Season, SeasonDto>();
            CreateMap<Episode, EpisodeDto>();

            CreateMap<CreateContentDto, Content>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (ContentType)src.Type));

            CreateMap<UpdateContentDto, Content>();
            
        }
    }
}
