using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StopStatAuth_6_0.Entities;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.DAL.Contracts;

namespace stutvds.DAL.Repositories.Contracts
{
    public interface ITriggerRepository : IBaseRepository<TriggerEntity>
    {
        Task<TriggerEntity> GetTriggerByNameAsync(string name);
        IEnumerable<TriggerEntity> GetDefaultTriggers(Language language);
        IEnumerable<TriggerEntity> GetTriggers(Guid userId, Language language);
    }
}