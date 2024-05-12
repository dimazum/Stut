using System.Collections.Generic;
using System.Linq;
using stutvds.Logic.DTOs;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Services.Tasks
{
    public class TriggerTaskManager
    {
        private readonly IEnumerable<ITriggerTask> _tasks;

        public TriggerTaskManager(IEnumerable<ITriggerTask> tasks)
        {
            _tasks = tasks;
        }

        public IEnumerable<TriggerTaskResult> GetTriggerTasks(string triggerValue)
        {
            var task1 = _tasks
                .First(x => x.Type == TriggerTaskType.Type1)
                .GetTask(triggerValue, columns: 1);
            
            var task2 = _tasks
                .First(x => x.Type == TriggerTaskType.Type2)
                .GetTask(triggerValue, 25, 3);
            
            var task3 = _tasks
                .First(x => x.Type == TriggerTaskType.Type3)
                .GetTask(triggerValue, 25, 3);
            
            var task4 = _tasks
                .First(x => x.Type == TriggerTaskType.Type4)
                .GetTask(triggerValue, 25, 3);

            yield return task1;
            yield return task2;
            yield return task3;
            yield return task4;
        }
    }
}