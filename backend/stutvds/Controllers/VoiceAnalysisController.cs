using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using stutvds.Controllers.Base;
using stutvds.DAL.Repositories;

namespace stutvds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoiceAnalysisController: BaseController
{
    private readonly VoiceAnalyseRepository _voiceAnalyseRepository;
    
    public VoiceAnalysisController(VoiceAnalyseRepository voiceAnalyseRepository)
    {
        _voiceAnalyseRepository = voiceAnalyseRepository;
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