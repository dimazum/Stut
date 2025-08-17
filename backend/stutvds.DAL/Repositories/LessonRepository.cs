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

        public async Task<DayLesson> GetByUserIdAndDay(Guid userId, DateTime date)
        {
            return await _dbContext.DayLessons
                .FirstOrDefaultAsync(x => x.UserId == userId &&
                                          x.Date.Year == date.Year &&
                                          x.Date.Month == date.Month &&
                                          x.Date.Day == date.Day);
        }
        
        public async Task<List<DayLesson>> GetAllByUserIdAndMonth(Guid userId, DateTime date)
        {
            return await _dbContext.DayLessons
                .Where(x => x.UserId == userId &&
                                          x.Date.Year == date.Year &&
                                          x.Date.Month == date.Month)
                .ToListAsync();
        }
    }
}