using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using StopStatAuth_6_0.Entities.Base;
using stutvds.DAL.Entities;

namespace StopStatAuth_6_0.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<TriggerEntity> Triggers { get; set; }
        public int WordsSpoken { get; set; }
        public int DayWordsLimit { get; set; } = 800;
        public List<LearnerTeacher> MyTeachers { get; set; }
        public List<LearnerTeacher> MyLearners { get; set; }
    }
}
