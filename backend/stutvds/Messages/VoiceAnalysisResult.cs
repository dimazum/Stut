using System;

namespace stutvds.Messages
{
    public class VoiceAnalysisResult
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }

        public double Duration { get; set; }
        public double PitchMean { get; set; }
        public double PitchStd { get; set; }
        public double PitchMin { get; set; }
        public double PitchMax { get; set; }
        public double VolumeMeanDb { get; set; }
        public double VolumeStdDb { get; set; }
        public double VolumePeakDb { get; set; }
        public double SpeechRate { get; set; }
        public double PauseRatio { get; set; }
        public double Jitter { get; set; }
        public double Shimmer { get; set; }
        public double[] MfccMean { get; set; }
    }
}