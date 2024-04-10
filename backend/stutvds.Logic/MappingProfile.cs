using AutoMapper;
using StopStatAuth_6_0.Entities;
using stutvds.Logic.DTOs;

namespace stutvds.Logic
{
    public class MappingProfile : Profile
    {
        /// <inheritdoc />
        public MappingProfile()
        {
            CreateMap<TriggerModel, TriggerEntity>().ReverseMap();
        }
    }
}