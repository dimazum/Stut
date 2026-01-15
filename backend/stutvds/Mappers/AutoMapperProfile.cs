using AutoMapper;
using StopStatAuth_6_0.Entities;
using stutvds.DAL.Entities;
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
            CreateMap<DayLesson, DailyLessonDto>().ReverseMap();
        }
    }
}