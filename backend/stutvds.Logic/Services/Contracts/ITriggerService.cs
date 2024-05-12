using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Logic.DTOs;

namespace stutvds.Logic.Services.Contracts
{
    public interface ITriggerService
    {
        /// <summary>
        /// Returns default triggers if not logged in, and user triggers if logged in
        /// </summary>
       
        TriggerModel GetRandomTrigger(Language language, Guid? userId);
        
        List<TriggerModel> GetTriggers(Language language, Guid? userId);
        
        List<TriggerModel> GetDefaultTriggers(Language language);
        
        Task<TriggerModel> GetTriggerByName(string name);
        Task DeleteAsync(string triggerValue);
        Task<TriggerModel> CreateAsync(TriggerModel triggerModel);
        Task UpdateTriggerAsync(TriggerModel triggerModel);
    }
}