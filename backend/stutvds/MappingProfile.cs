using AutoMapper;
using StopStatAuth_6_0.Entities;
using stutvds.DAL.Entities;
using stutvds.Logic.DTOs;
using stutvds.Models.ClientDto;

namespace stutvds
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TriggerClientDto, TriggerEntity>();
            CreateMap<TriggerEntity, TriggerResultClientDto>();
            CreateMap<ArticleEntity, ArticleDto>().ReverseMap();
        }
    }
}