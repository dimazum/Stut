using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stutvds.Data;

namespace stutvds.DAL.Repositories;

public class VoiceAnalyseRepository : BaseRepository<VoiceAnalysisEntity>
{
    public VoiceAnalyseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<List<VoiceAnalysisEntity>> GetByUserIdAsync(Guid userId)
    {
        return await _dbContext
            .Set<VoiceAnalysisEntity>()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }
}