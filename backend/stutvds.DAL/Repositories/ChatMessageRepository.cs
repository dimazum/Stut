using stutvds.DAL.Entities;
using stutvds.Data;

namespace stutvds.DAL.Repositories;

public class ChatMessageRepository : BaseRepository<ChatMessage>
{
    public ChatMessageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}