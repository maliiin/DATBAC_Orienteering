using AutoMapper;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Navigation;
using orienteering_backend.Core.Domain.Navigation.Dto;
using orienteering_backend.Core.Domain.Quiz;
using orienteering_backend.Core.Domain.Quiz.Dto;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Core.Domain.Track.Dto;
using NavigationClass=orienteering_backend.Core.Domain.Navigation.Navigation;

// License Automapper (MIT): https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt
// Kilder: https://github.com/hgmauri/sample-automapper/blob/main/src/Sample.Automapper.Application/MapperProfile.cs (03.03.2023)

namespace orienteering_backend.Infrastructure.Automapper
{

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<QuizQuestion, QuizQuestionDto>().ForMember(
                    dest => dest.QuizQuestionId,
                    opt => opt.MapFrom(src => $"{src.Id}")
                ).ForMember(dest => dest.Alternatives, opt => opt.MapFrom(src => src.Alternatives));
            CreateMap<Alternative, AlternativeDto>();
            CreateMap<Checkpoint, CheckpointDto>().ReverseMap();

            CreateMap<Checkpoint, CheckpointNameAndQRCodeDto>().ReverseMap();
            CreateMap<Track, TrackDto>()
                .ForMember(dest => dest.TrackId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Name)).ReverseMap();
            CreateMap<NavigationImage, NavigationImageDto>()
                .ConstructUsing(src => new NavigationImageDto(src.Order));

            CreateMap<NavigationClass, NavigationDto>()
                .ConstructUsing(src => new NavigationDto(src.ToCheckpoint));
            CreateMap<Track, TrackUserIdDto>()
                .ForMember(dest => dest.TrackId, opt => opt.MapFrom(src => src.Id));
            CreateMap<CreateTrackDto, Track>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TrackName));
        }
    }

}