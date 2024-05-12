using AutoMapper;
using stutvds.DAL.Entities;
using stutvds.Logic.DTOs;

namespace stutvds.Logic
{
    public class LogicMappingProfile : Profile
    {
        /// <inheritdoc />
        public LogicMappingProfile()
        {
            CreateMap<TriggerModel, TriggerEntity>().ReverseMap();
        }
    }
}