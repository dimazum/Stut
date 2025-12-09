using stutvds.Data;

namespace stutvds.DAL.Repositories;

public class VoiceAnalyseRepository : BaseRepository<VoiceAnalysisEntity>
{
    public VoiceAnalyseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}