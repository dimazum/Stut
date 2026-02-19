using System.Linq;
using stutvds.Logic.Data;

namespace stutvds.Logic.Services
{
    public class TriggerService
    {
        public string GetRandomTriggers()
        {
            return Triggers.GetRandomTriggers(1).First();
        }
    }
}