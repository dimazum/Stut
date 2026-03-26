using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using stutvds.Data;

public interface IStoredProcedureInstaller
{
    Task InstallAsync();
}

public class StoredProcedureInstaller : IStoredProcedureInstaller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;

    public StoredProcedureInstaller(ApplicationDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task InstallAsync()
    {
        var root = Directory.GetParent(_env.ContentRootPath)!.FullName;
        var folder = Path.Combine(root, "stutvds.DAL", "StoredProcedures");

        if (!Directory.Exists(folder))
            return;

        var files = Directory.GetFiles(folder, "*.sql", SearchOption.AllDirectories)
            .OrderBy(f => f);

        foreach (var file in files)
        {
            var script = await File.ReadAllTextAsync(file);

            // поддержка GO как в SSMS
            var batches = Regex.Split(script, @"^\s*GO\s*$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (var batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch))
                    continue;

                await _db.Database.ExecuteSqlRawAsync(batch);
            }
        }
    }
}