using AutoMapper;
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
        }
    }
}