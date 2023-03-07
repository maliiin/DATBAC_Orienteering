﻿using AutoMapper;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Quiz;
using orienteering_backend.Core.Domain.Quiz.Dto;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Core.Domain.Track.Dto;

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
            CreateMap<QuizQuestion, QuizQuestionDto>().ForMember(
                    dest => dest.QuizQuestionId,
                    opt => opt.MapFrom(src => $"{src.Id}")
                ).ForMember(dest => dest.Alternatives, opt => opt.MapFrom(src => src.Alternatives));
            CreateMap<Alternative, AlternativeDto>();
            CreateMap<Checkpoint, CheckpointDto>().ReverseMap();
            CreateMap<Checkpoint, CheckpointNameAndQRCodeDto>().ReverseMap();
            CreateMap<Checkpoint, CheckpointNameAndQRCodeDto>().ReverseMap();
            CreateMap<Quiz, QuizDto>()
                .ForMember(dest => dest.QuizId,
                    opt => opt.MapFrom(src => $"{src.Id}"))
                .ForMember(dest => dest.QuizQuestions, opt => opt.MapFrom(src => src.QuizQuestions));
            CreateMap<Track, TrackDto>()
                .ForMember(dest => dest.TrackId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Name)).ReverseMap();


        }
    }

}