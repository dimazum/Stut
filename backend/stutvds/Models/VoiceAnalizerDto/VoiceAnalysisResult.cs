using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace stutvds.Models.VoiceAnalizerDto;

public class VoiceAnalysisResult
{
    [JsonPropertyName("mean_pitch")]
    public double? MeanPitch { get; set; }

    [JsonPropertyName("pitch_min")]
    public double? PitchMin { get; set; }

    [JsonPropertyName("pitch_max")]
    public double? PitchMax { get; set; }

    [JsonPropertyName("volume_db")]
    public double VolumeDb { get; set; }

    [JsonPropertyName("mfcc_mean")]
    public List<double> MfccMean { get; set; } = new();
}