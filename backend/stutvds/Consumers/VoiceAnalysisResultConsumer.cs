using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using stutvds.Messages;
using stutvds.Models.ClientDto;
using stutvds.WebSocketHubs;

namespace stutvds.Consumers;

public class VoiceAnalysisResultConsumer : IConsumer<VoiceAnalysisResult>
{
    private readonly IHubContext<VoiceAnalysisHub> _hubContext;

    public VoiceAnalysisResultConsumer(IHubContext<VoiceAnalysisHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<VoiceAnalysisResult> context)
    {
        var result = context.Message;

        var updateDto = new VoiceAnalysisUpdateDto
        {
            SessionId = result.SessionId,
            Jitter =  Math.Round(result.Jitter, 4),
            Shimmer =  Math.Round(result.Shimmer, 4)
        };

        
        var userId = result.UserId;
        await _hubContext.Clients.Group(userId.ToString())
            .SendAsync("UpdateVoiceAnalysis", updateDto);
    }
}

