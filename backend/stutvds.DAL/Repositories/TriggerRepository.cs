using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StopStatAuth_6_0.Entities;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.DAL.Contracts;
using stutvds.DAL.Entities;
using stutvds.DAL.Repositories;
using stutvds.DAL.Repositories.Contracts;
using stutvds.Data;

namespace stutvds.DAL
{
	public class TriggerRepository : BaseRepository<TriggerEntity>
	{
		public TriggerRepository(ApplicationDbContext dbContext): base(dbContext)
		{
		}

		public async Task<TriggerEntity> GetTriggerByNameAsync(string name, Guid userId)
		{
			return await _dbContext.Triggers.FirstOrDefaultAsync(t => t.Value == name && t.UserId == userId );
		}
		
		public async Task<TriggerEntity> GetByName(string name, Guid userId)
		{
			return await _dbContext.Triggers.FirstAsync(t => t.Value == name && t.UserId == userId );
		}

		public IEnumerable<TriggerEntity> GetDefaultTriggers(Language language)
		{
			return _dbContext.Triggers
				.Where(t => t.IsDefault == true)
				.Where(t => t.TriggerType == TriggerType.Short)
				.Where(t => t.Language == language)
				.OrderBy(t => t.Value)
				.ToList();
		}

		public IEnumerable<TriggerEntity> GetTriggers(Guid userId, Language language)
		{
			return _dbContext.Triggers
				.Where(t => t.Language == language)
				.Where(t => t.UserId == userId)
				.OrderBy(t => t.Value)
				.ToList();
		}
	}
}
