using AutoMapper;
using orienteering_backend.Core.Domain.Quiz;
using orienteering_backend.Core.Domain.Quiz.Dto;
using System.Text.RegularExpressions;

// Kilder: https://github.com/hgmauri/sample-automapper/blob/main/src/Sample.Automapper.Application/MapperProfile.cs (03.03.2023)

namespace orienteering_backend.Infrastructure.Automapper
{

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            //Alternative
            //CreateMap<Alternative, AlternativeDto>().ReverseMap();

            ////Group
            //CreateMap<Group, GroupViewModel>().ReverseMap()
            //    .ForMember(dest => dest.Description, src => src.Ignore())
            //    .AfterMap((src, dest) =>
            //    {
            //        dest.CreatedAt = DateTime.Now;
            //        dest.Description = dest.Name;
            //    });

            CreateMap<QuizQuestion, NextQuizQuestionDto>().ForMember(
                    dest => dest.QuizQuestionId,
                    opt => opt.MapFrom(src => $"{src.Id}")
                )
                .ForMember(dest => dest.CorrectAlternative, opt => opt.Ignore()
                );
            //CreateMap<NextQuizQuestionDto, QuizQuestion>().ForMember(
            //        dest => dest.Id,
            //        opt => opt.MapFrom(src => $"{src.QuizQuestionId}")
            //    );
            CreateMap<Alternative, AlternativeDto>();


        }
    }

}