using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stutvds.DAL.Entities;
using stutvds.Data;

namespace stutvds.DAL.Repositories;

public class HistogramRepository : BaseRepository<Histogram>
{
    public HistogramRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<Histogram> AddOrUpdate(Histogram histogram)
    {
        var hist = await _dbContext.Histograms.AsNoTracking()
            .FirstOrDefaultAsync(h => h.Name == histogram.Name);

        if (hist == null)
        {
           return await AddAsync(histogram);
        }
        else
        {
            return await UpdateByName(histogram);
        }
    }
    public async Task<Histogram> UpdateByName(Histogram histogram)
    {
        var dbHistogram = await _dbContext.Histograms
            .Include(h => h.Chars)
            .FirstOrDefaultAsync(h => h.Name == histogram.Name);

        if (dbHistogram != null)
        {
            foreach (var c in histogram.Chars)
            {
                var dbChar = dbHistogram.Chars.FirstOrDefault(x => x.Id == c.Id);
                if (dbChar != null)
                {
                    dbChar.Char = c.Char;
                    dbChar.Air = c.Air;
                    dbChar.Order = c.Order;
                }
                else
                {
                    dbHistogram.Chars.Add(c);
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        return dbHistogram;
    }
    

    public async Task<Histogram> GetWithCharmsByName(string name)
    {
        return await _dbContext.Histograms
            .Include(h => h.Chars.OrderBy(x=>x.Order))
            .FirstOrDefaultAsync(h => h.Name == name);
    }
}