
using AutoMapper;

namespace WatchCenter.Application.Mapping
{
    internal class GenreMapping : Profile
    {
        public GenreMapping()
        {
            CreateMap<Genre,GenreDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
