using System;

namespace stutvds.Messages;

public record VoiceAnalysisRequested
{
    public Guid SessionId { get; init; }
    public Guid UserId { get; init; }
    public int ChunkIndex { get; init; }
    public string FileName { get; init; }
    public string ContentType { get; init; }
    public byte[] Data { get; init; }
}
