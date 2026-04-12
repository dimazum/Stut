using StopStatAuth_6_0.Entities;

public class LearnerTeacher
{
    public string LearnerId { get; set; }      // кого учат
    public ApplicationUser Learner { get; set; }
    public string TeacherId { get; set; }    // кто учит
    public ApplicationUser Teacher { get; set; }
}