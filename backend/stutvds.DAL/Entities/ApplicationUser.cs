using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace StopStatAuth_6_0.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<TriggerEntity> Triggers { get; set; }

        public int WordsSpoken { get; set; }

        public int DayWordsLimit { get; set; } = 800;
	}
}
