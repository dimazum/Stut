using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stutvds.Controllers.Base;
using stutvds.DAL.Repositories;
using stutvds.Messages;

namespace stutvds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoiceAnalysisController: BaseController
{
    private readonly VoiceAnalyseRepository _voiceAnalyseRepository;
    private readonly IPublishEndpoint _publisher;
    
    public VoiceAnalysisController(
        VoiceAnalyseRepository voiceAnalyseRepository,
        IPublishEndpoint publisher)
    {
        _voiceAnalyseRepository = voiceAnalyseRepository;
        _publisher = publisher;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is empty");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var message = new VoiceAnalysisRequested
        {
            SessionId = Guid.NewGuid(),
            UserId = UserId,
            ChunkIndex = 1,
            FileName = file.FileName,
            ContentType = file.ContentType,
            Data = ms.ToArray()
        };
        

        await _publisher.Publish(message);

        return Accepted();
    }

    /// <summary>
    /// Войс анализы юзера
    /// </summary>
    [HttpGet("last")]
    public async Task<IActionResult> UserList()
    {
        List<VoiceAnalysisEntity> result = await _voiceAnalyseRepository.GetByUserIdAsync(UserId);
        return Ok(result);
    }
}