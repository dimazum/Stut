using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stutvds.DAL.Entities;
using stutvds.Data;

namespace stutvds.DAL.Repositories
{
    public class DayLessonRepository: BaseRepository<DayLesson>
    {
        private readonly ApplicationDbContext _dbContext;

        public DayLessonRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DayLesson> GetByUserIdAndDay(Guid userId, DateTimeOffset date)
        {
            return await _dbContext.DayLessons
                .FirstOrDefaultAsync(x => x.UserId == userId &&
                                          x.StartTime.Year == date.Year &&
                                          x.StartTime.Month == date.Month &&
                                          x.StartTime.Day == date.Day);
        }
        
        public async Task<List<DayLesson>> GetAllByUserIdAndMonth(Guid userId, DateTimeOffset date)
        {
            return await _dbContext.DayLessons
                .Where(x => x.UserId == userId &&
                                          x.StartTime.Year == date.Year &&
                                          x.StartTime.Month == date.Month)
                .ToListAsync();
        }
    }
}