// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using stutvds.Models.ClientDto;
//
// namespace stutvds.Controllers
// {
//     public class TriggerController : Controller
//     {
//         private readonly ITriggerService _triggerService;
//
//         public IEnumerable<TriggerEntity> Triggers;
//
//         public TriggerController(ITriggerService triggerService)
//         {
//             ILogger logger;
//             _triggerService = triggerService;
//         }
//
//         [HttpGet]
//         public async Task<JsonResult> GetTriggerTask(string trigger, string title)
//         {
//             var task0 = new TriggerTask() { Content = GenerateStr0(trigger, 5) };
//             var task1 = new TriggerTask() { Content = GenerateStr1(trigger, 15) };
//             var task2 = new TriggerTask() { Content = GenerateStr2(trigger, 10) };
//             var task3 = new TriggerTask() { Content = GenerateStr3(trigger, 15) };
//             var task4 = new TriggerTask() { Content = GenerateStr4(trigger, 10) };
//
//
//             var resp = new TriggerTaskResponse()
//             {
//                 Tasks = new List<TriggerTask> { task0, task1, task2, task4, task3 },
//             };
//
//             return new JsonResult(resp);
//         }
//
//
//         [HttpPost]
//         public async Task<JsonResult> Create(string trigger, int difficulty)
//         {
//             var triggerModel = new TriggerModel()
//             {
//                 Trigger = trigger,
//                 Difficulty = difficulty
//             };
//
//             await _triggerService.CreateAsync(triggerModel);
//
//             var triggers = _triggerService.GetTriggers();
//
//             return new JsonResult(triggers.Select(t => new
//             {
//                 t.Trigger,
//                 CreatedAt = t.CreatedAt.DateTimeToShortString(),
//                 t.Difficulty
//             }));
//         }
//
//         [HttpPost]
//         public async Task<JsonResult> Delete(string trigger)
//         {
//             var triggerModel = new TriggerModel()
//             {
//                 Trigger = trigger,
//             };
//
//             await _triggerService.DeleteAsync(triggerModel);
//
//             var triggers = _triggerService.GetTriggers();
//
//             return new JsonResult(triggers.Select(t => new
//             {
//                 t.Trigger,
//                 CreatedAt = t.CreatedAt.DateTimeToShortString(),
//                 t.Difficulty
//             }));
//         }
//
//         [HttpGet]
//         public JsonResult Get()
//         {
//             var triggers = _triggerService.GetTriggers();
//
//             return new JsonResult(triggers.Select(t => new
//             {
//                 t.Trigger,
//                 CreatedAt = t.CreatedAt.DateTimeToShortString(),
//                 t.Difficulty
//             }));
//         }
//
//         [HttpPost]
//         public async Task<JsonResult> ChangeDifficulty(string trigger, int difficulty)
//         {
//             var triggerModel = new TriggerModel()
//             {
//                 Trigger = trigger,
//                 Difficulty = difficulty
//             };
//
//             await _triggerService.UpdateTriggerAsync(triggerModel);
//
//             var triggers = _triggerService.GetTriggers();
//
//             return new JsonResult(triggers.Select(t => new
//             {
//                 t.Trigger,
//                 CreatedAt = t.CreatedAt.DateTimeToShortString(),
//                 t.Difficulty
//             }));
//         }
//
//         //алиса
//         private IEnumerable<string> GenerateStr0(string trigger, int repeatVal = 10)
//         {
//             for (int i = 0; i < repeatVal; i++)
//             {
//                 yield return $"{trigger}, {trigger}, {trigger}, {trigger}";
//             }
//         }
//
//
//         //ээц - апип, ээц - апип, ээц - апип, ээц - апип
//         private IEnumerable<string> GenerateStr1(string trigger, int repeatVal = 10)
//         {
//             var lg = new LetterGenerator(LetterVariant.Vowels1);
//             var vg = new VowelGenerator();
//             var cg = new ConsonantGenerator();
//
//             for (int i = 0; i < repeatVal; i++)
//             {
//                 var v1 = lg.GetRandomUnique();
//                 var v2 = vg.GetRandomUnique();
//                 var c = cg.GetRandomUnique();
//
//                 yield return
//                     $"{v1}{v2}{c} - {trigger}, {v1}{v2}{c} - {trigger}, {v1}{v2}{c} - {trigger}, {v1}{v2}{c} - {trigger}";
//             }
//         }
//
//         //алиса - каче
//         private IEnumerable<string> GenerateStr2(string trigger, int repeatVal = 10)
//         {
//             var cg = new LetterGenerator(LetterVariant.Consonants);
//             var lg = new LetterGenerator(LetterVariant.AllVowels);
//
//
//             for (int i = 0; i < repeatVal; i++)
//             {
//                 var c1 = cg.GetRandomUnique();
//                 var c2 = cg.GetRandomUnique();
//                 var c3 = cg.GetRandomUnique();
//                 var c4 = cg.GetRandomUnique();
//
//                 var v1 = lg.GetRandomUnique();
//                 var v2 = lg.GetRandomUnique();
//                 var v3 = lg.GetRandomUnique();
//                 var v4 = lg.GetRandomUnique();
//                 var v5 = lg.GetRandomUnique();
//
//                 yield return
//                     $"{v1}{v2}{c1} - {v3}{c2} - {trigger}, {v1}{v2}{c1} - {v3}{c2} - {trigger}, {v1}{v2}{c1} - {v3}{c2} - {trigger},^^";
//             }
//         }
//
//         //вде - алиса - ист
//         private IEnumerable<string> GenerateStr3(string trigger, int repeatVal = 10)
//         {
//             var vg = new VowelGenerator();
//             var cg = new ConsonantGenerator();
//
//             for (int i = 0; i < repeatVal; i++)
//             {
//                 var v1 = vg.GetRandomUnique();
//                 var v2 = vg.GetRandomUnique();
//                 var v3 = vg.GetRandomUnique();
//                 var c1 = cg.GetRandomUnique();
//                 var c2 = cg.GetRandomUnique();
//                 var c3 = cg.GetRandomUnique();
//
//                 yield return
//                     $" {c1}{c2}{v1}-{trigger}-{v2}{c1}{c3}, {c1}{c2}{v1}-{trigger}-{v2}{c1}{c3}, {c1}{c2}{v1}-{trigger}-{v2}{c1}{c3}";
//             }
//         }
//
//         //мцис-алиса
//         private IEnumerable<string> GenerateStr4(string trigger, int repeatVal = 10)
//         {
//             var vg = new VowelGenerator();
//             var cg = new ConsonantGenerator();
//
//
//             for (int i = 0; i < repeatVal; i++)
//             {
//                 var v1 = vg.GetRandomUnique();
//                 var c1 = cg.GetRandomUnique();
//                 var c2 = cg.GetRandomUnique();
//                 var c3 = cg.GetRandomUnique();
//
//                 yield return
//                     $"{trigger} - {c1}{c2}{v1}{c3}, {trigger} - {c1}{c2}{v1}{c3}, {trigger} - {c1}{c2}{v1}{c3}, {trigger} - {c1}{c2}{v1}{c3},";
//             }
//         }
//     }
// }