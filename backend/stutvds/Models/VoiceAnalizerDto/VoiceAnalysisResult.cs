using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace stutvds.Models.VoiceAnalizerDto;

public class VoiceAnalysisResult
{
    // ===== Duration =====
    [JsonPropertyName("duration")]
    public double Duration { get; set; }

    // ===== Pitch =====
    [JsonPropertyName("pitch_mean")]
    public double? PitchMean { get; set; }

    [JsonPropertyName("pitch_std")]
    public double? PitchStd { get; set; }

    [JsonPropertyName("pitch_min")]
    public double? PitchMin { get; set; }

    [JsonPropertyName("pitch_max")]
    public double? PitchMax { get; set; }

    // ===== Volume =====
    [JsonPropertyName("volume_mean_db")]
    public double VolumeMeanDb { get; set; }

    [JsonPropertyName("volume_std_db")]
    public double VolumeStdDb { get; set; }

    [JsonPropertyName("volume_peak_db")]
    public double VolumePeakDb { get; set; }

    // ===== Rhythm / Timing =====
    [JsonPropertyName("speech_rate")]
    public double SpeechRate { get; set; }

    [JsonPropertyName("pause_ratio")]
    public double PauseRatio { get; set; }

    // ===== Voice Quality =====
    [JsonPropertyName("jitter")]
    public double? Jitter { get; set; }

    [JsonPropertyName("shimmer")]
    public double? Shimmer { get; set; }

    // ===== Timbre =====
    [JsonPropertyName("mfcc_mean")]
    public List<double> MfccMean { get; set; } = new();
}
