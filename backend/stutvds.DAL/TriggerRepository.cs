using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StopStatAuth_6_0.Entities;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.DAL.Contracts;
using stutvds.DAL.Repositories;
using stutvds.DAL.Repositories.Contracts;
using stutvds.Data;

namespace stutvds.DAL
{
	public class TriggerRepository : BaseRepository<TriggerEntity>, ITriggerRepository
	{
		public TriggerRepository(ApplicationDbContext dbContext): base(dbContext)
		{
		}

		public async Task<TriggerEntity> GetTriggerByNameAsync(string name)
		{
			return await _dbContext.Triggers.FirstOrDefaultAsync(t => t.Value == name);
		}

		public IEnumerable<TriggerEntity> GetDefaultTriggers(Language language)
		{
			return _dbContext.Triggers
				.Where(t => t.IsDefault == true)
				.Where(t => t.TriggerType == TriggerType.Short)
				.Where(t => t.Language == language)
				.OrderBy(t => t.Value);
		}

		public IEnumerable<TriggerEntity> GetTriggers(Guid userId, Language language)
		{
			return _dbContext.Triggers
				.Where(t => t.IsDefault == false)
				.Where(t => t.Language == language)
				.Where(t => t.UserId == userId)
				.Where(t => t.TriggerType == TriggerType.Short)
				.OrderBy(t => t.Value);
		}
	}
}
