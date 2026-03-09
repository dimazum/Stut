using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using StopStatAuth_6_0.Entities.Base;

public class VoiceAnalysisEntity : Entity
{
    // ===== Meta =====
    public Guid UserId { get; set; }

    public DateTimeOffset RecordedAt { get; set; } = DateTimeOffset.UtcNow;

    public double Duration { get; set; }

    // ===== Pitch =====
    public double? PitchMean { get; set; }

    public double? PitchStd { get; set; }

    public double? PitchMin { get; set; }

    public double? PitchMax { get; set; }

    // ===== Volume =====
    public double VolumeMeanDb { get; set; }

    public double VolumeStdDb { get; set; }

    public double VolumePeakDb { get; set; }

    // ===== Rhythm / Timing =====
    /// <summary>
    /// Onsets per second (relative speech rate)
    /// </summary>
    public double SpeechRate { get; set; }

    /// <summary>
    /// Silence ratio 0..1
    /// </summary>
    public double PauseRatio { get; set; }

    // ===== Voice Quality =====
    public double? Jitter { get; set; }

    public double? Shimmer { get; set; }

    // ===== MFCC =====
    public string MfccJson { get; set; } = string.Empty;

    [NotMapped]
    public List<double> MfccMean
    {
        get => string.IsNullOrWhiteSpace(MfccJson)
            ? new List<double>()
            : JsonSerializer.Deserialize<List<double>>(MfccJson)!;
        set => MfccJson = JsonSerializer.Serialize(value);
    }
}