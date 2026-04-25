using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StopStatAuth_6_0.Entities;
using stutvds.Data;

namespace stutvds.DAL.Repositories;

public class UserRepository 
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ApplicationUser>> GetUserByName(string name)
    {
        var lowerName = name.ToLower();
        return await _dbContext.Users
            .Where(x => x.UserName.ToLower().Contains(lowerName))
            .ToListAsync();
    }
}