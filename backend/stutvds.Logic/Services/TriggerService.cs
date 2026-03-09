using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.DAL;
using stutvds.Logic.Data;

namespace stutvds.Logic.Services
{
    public class TriggerService
    {
        private readonly TriggerRepository _triggerRepository;

        public TriggerService(TriggerRepository triggerRepository)
        {
            _triggerRepository = triggerRepository;
        }
        
        public List<string> GetRandomTriggers(int count)
        {
            return Triggers.GetRandomTriggers(count);
        }

        public async Task<string> GetRandomTriggerValue(Guid userId, Language language)
        {
            string triggerVal;
            
            var trigger = await _triggerRepository.GetRandomTrigger(userId, language);

            if (trigger != null)
            {
                triggerVal = trigger.Value;
            }
            else
            {
                triggerVal = GetRandomTriggers(1).First();
            }

            return triggerVal;
        }
    }
}