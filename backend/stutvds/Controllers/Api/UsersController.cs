using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stutvds.Controllers.Base;
using stutvds.DAL.Repositories;
using stutvds.Data;
using stutvds.Logic.DTOs;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers.Api;

[ApiController]
[Route("api/users")]
public class UsersController : BaseController
{
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public UsersController(UserRepository userRepository, IMapper mapper, ApplicationDbContext context)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _context = context;
    }

    /// GET /api/users?name=alex
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> SearchUsers([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Ok(new List<UserDto>());

        var users = await _userRepository.GetUserByName(name);

        var mapped = _mapper.Map<List<UserDto>>(users);
        
        return Ok(mapped);
    }
    
    [HttpPost("belearner")]
    public async Task<IActionResult> BeLearner([FromBody] AddLearnerDto dto)
    {
        var learnerId = GetUserId();
        
        if ( dto.TeacherId == learnerId)
            return BadRequest("Нельзя стать учеником у самого себя");

        var exists = await _context.LearnerTeachers.AnyAsync(x =>
            x.LearnerId == learnerId &&
            x.TeacherId == dto.TeacherId);

        if (exists)
        {
            return BadRequest(ErrorCodes.AlreadyLearner);
        }

        var relation = new LearnerTeacher
        {
            LearnerId = learnerId,
            TeacherId = dto.TeacherId
        };

        _context.LearnerTeachers.Add(relation);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
