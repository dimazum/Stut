using AutoMapper;
using stutvds.Logic.DTOs;
using stutvds.Models.ClientDto;

namespace stutvds
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TriggerClientDto, TriggerModel>();
            CreateMap<TriggerModel, TriggerResultClientDto>();
        }
    }
}