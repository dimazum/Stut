using System.Collections.Generic;

namespace SpeechApp.Models
{
    public class ArticulationCategory
    {
        public string Category { get; set; }
        public List<string> Sounds { get; set; }
        public string Goal { get; set; }
        public List<ExerciseGroup> Exercises { get; set; }
    }

    public class ExerciseGroup
    {
        public string Group { get; set; }
        public string Purpose { get; set; }
        public List<Exercise> Exercises { get; set; }
    }

    public class Exercise
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Instruction { get; set; }
        public int? DurationSeconds { get; set; }
        public int? Repetitions { get; set; }
        public List<string> Sounds { get; set; }
        public List<string> Tips { get; set; }
        public List<string> CommonMistakes { get; set; }
    }
}