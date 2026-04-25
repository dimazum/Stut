using AutoMapper;
using StopStatAuth_6_0.Entities;
using stutvds.DAL.Entities;
using stutvds.Logic.DTOs;
using stutvds.Models.ClientDto;

namespace stutvds
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TriggerClientDto, TriggerEntity>();
            CreateMap<TriggerEntity, TriggerResultClientDto>();
            CreateMap<ArticleEntity, ArticleDto>().ReverseMap();
            CreateMap<UserDto, ApplicationUser>().ReverseMap();
            CreateMap<DayLesson, DailyLessonDto>().ReverseMap();
            CreateMap<Histogram, HistogramDto>().ReverseMap();
            CreateMap<CharItem, CharDto>()
                .ReverseMap();
        }
    }
}