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
        
        public async Task<List<DayLesson>> GetAllByUserIdAndMonth(Guid userId, int year, int month)
        {
            return await _dbContext.DayLessons
                .Where(x => x.UserId == userId &&
                                          x.StartTime.Year == year &&
                                          x.StartTime.Month == month)
                .ToListAsync();
        }
        
        
        /// <summary>
        /// Стрик по завершенным урокам
        /// </summary>
        public async Task<List<DateTime>> GetLastLessonDates(Guid userId, int days)
        {
            var todayUtc = DateTimeOffset.Now.Date;
            var fromDate = todayUtc.AddDays(-days);

            return await _dbContext.DayLessons
                .Where(x =>
                    x.UserId == userId &&
                    x.FinishTime.HasValue &&
                    x.FinishTime.Value.Date >= fromDate)
                .Select(x => x.FinishTime.Value.Date)
                .Distinct()
                .OrderByDescending(x => x)
                .ToListAsync();
        }
        
        public async Task<int> GetNotRewardedPoints(Guid userId)
        {
            return await _dbContext.DayLessons
                .Where(x =>
                    x.UserId == userId &&
                    !x.Rewarded)
                .SumAsync(x => x.RewardPoints);
        }
    }
}