using System;

namespace stutvds.Models.ClientDto;

public class VoiceAnalysisUpdateDto
{
    public Guid SessionId { get; set; }
    public double Jitter { get; set; }
    public double Shimmer { get; set; }
}
