using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using StopStatAuth_6_0.Entities.Base;

public class VoiceAnalysisEntity : Entity
{
    public Guid UserId { get; set; }
    public DateTimeOffset RecordedAt { get; set; } = DateTimeOffset.Now;
    public double? MeanPitch { get; set; }
    public double? PitchMin { get; set; }
    public double? PitchMax { get; set; }
    public double VolumeDb { get; set; }
    public string MfccJson { get; set; } = string.Empty;

    [NotMapped]
    public List<double> MfccMean
    {
        get => string.IsNullOrEmpty(MfccJson)
            ? new List<double>()
            : JsonSerializer.Deserialize<List<double>>(MfccJson)!;
        set => MfccJson = JsonSerializer.Serialize(value);
    }
}