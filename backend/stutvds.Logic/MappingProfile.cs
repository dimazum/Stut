using AutoMapper;
using stutvds.DAL.Entities;
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