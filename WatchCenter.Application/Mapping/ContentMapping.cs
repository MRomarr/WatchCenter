
using AutoMapper;

namespace WatchCenter.Application.Mapping
{
    internal class ContentMapping : Profile
    {
        public ContentMapping()
        {
            CreateMap<Content, ContentDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type));

            CreateMap<CreateContentDto, Content>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src=>src.GenreId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src=>src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src=>src.Description))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (ContentType)src.Type));
            
        CreateMap<UpdateContentDto, Content>();
            
        }
    }
}
