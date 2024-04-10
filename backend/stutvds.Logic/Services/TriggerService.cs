using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StopStatAuth_6_0.Entities;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.DAL.Repositories.Contracts;
using stutvds.Logic.DTOs;
using stutvds.Logic.Services.Contracts;

namespace stutvds.Logic.Services
{
    public class TriggerService : ITriggerService
    {
        private readonly ITriggerRepository _triggerRepository;
        private readonly IMapper _mapper;

        public TriggerService(ITriggerRepository triggerRepository, IMapper mapper)
        {
            _triggerRepository = triggerRepository;
            _mapper = mapper;
        }

        public TriggerModel GetRandomTrigger(Language language, Guid? userId)
        {
            var triggers = GetTriggers(language, userId);
            var random = new Random();
            var index = random.Next(0, triggers.Count);
            return triggers.ToArray()[index];
        }

        public List<TriggerModel> GetTriggers(Language language, Guid? userId)
        {
            if (userId == null)
            {
                return GetDefaultTriggers(language);
            }
            else
            {
                return GetUserTriggers(language, userId.Value);
            }
        }

        public List<TriggerModel> GetDefaultTriggers(Language language)
        {
            var triggers = _triggerRepository.GetDefaultTriggers(language);

            var mapped = _mapper.Map<List<TriggerModel>>(triggers);
            return mapped;
        }

        public async Task DeleteAsync(TriggerModel triggerModel)
        {
            var deletedProduct = await _triggerRepository.GetTriggerByNameAsync(triggerModel.Trigger);
            if (deletedProduct == null)
                throw new ApplicationException($"Entity could not be loaded.");

            await _triggerRepository.DeleteAsync(deletedProduct);
        }

        public async Task<TriggerModel> CreateAsync(TriggerModel triggerModel)
        {
            triggerModel.CreatedAt = DateTime.UtcNow;
            triggerModel.IsDefault = !triggerModel.UserId.HasValue;

            var mappedEntity = _mapper.Map<TriggerEntity>(triggerModel);

            mappedEntity.CreatedAt = DateTime.UtcNow;
            mappedEntity.IsDefault = !triggerModel.UserId.HasValue;

            if (mappedEntity == null)
            {
                throw new ApplicationException($"Entity could not be mapped.");
            }

            var newEntity = await _triggerRepository.AddAsync(mappedEntity);

            var newMappedEntity = _mapper.Map<TriggerModel>(newEntity);

            return newMappedEntity;
        }

        public async Task UpdateTriggerAsync(TriggerModel triggerModel)
        {
            var triggerEntity = await _triggerRepository.GetTriggerByNameAsync(triggerModel.Trigger);
            triggerEntity.Difficulty = triggerModel.Difficulty;

            await _triggerRepository.UpdateAsync(triggerEntity);
        }

        public async Task<TriggerModel> GetTriggerByName(string name)
        {
            var triggerEntity = await _triggerRepository.GetTriggerByNameAsync(name);

            var mapped = _mapper.Map<TriggerModel>(triggerEntity);

            return mapped;
        }

        private List<TriggerModel> GetUserTriggers(Language language, Guid userId)
        {
            var triggers = _triggerRepository.GetTriggers(userId, language);
            var mapped = _mapper.Map<List<TriggerModel>>(triggers);

            return mapped;
        }
    }
}