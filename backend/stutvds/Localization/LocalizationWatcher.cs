using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace stutvds.Localization;

public class LocalizationWatcher : IHostedService
{
    private string _localizationFilesPath;
    private DateTime _lastRun = DateTime.MinValue;
    private int _fileChangesDelay = 1000;
    
    private Timer? _debounceTimer;
    private readonly object _lock = new();
    
    private readonly ILocalizationVersionStore _store;
    private readonly LocalizationVersionCalculator _calculator;
    private readonly FileSystemWatcher _watcher;
    private readonly IHubContext<TranslationsHub> _hub;

    public LocalizationWatcher(
        ILocalizationVersionStore store,
        LocalizationVersionCalculator calculator,
        IWebHostEnvironment env,
        IHubContext<TranslationsHub> hub)
    {
        _store = store;
        _calculator = calculator;
        _hub = hub;

        var path = Path.Combine(env.ContentRootPath, @"Localization\Resources");
        
        _localizationFilesPath = path;

        _watcher = new FileSystemWatcher(path, "*.resx")
        {
            IncludeSubdirectories = true,
            EnableRaisingEvents = false
        };

        _watcher.Changed += OnChanged;
        _watcher.Created += OnChanged;
        _watcher.Deleted += OnChanged;
        _watcher.Renamed += OnChanged;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Recalculate();
        _watcher.EnableRaisingEvents = true;
        
        return Task.CompletedTask;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        lock (_lock)
        {
            _debounceTimer?.Dispose();

            _debounceTimer = new Timer(async _ =>
            {
                await HandleChange();
            }, null, _fileChangesDelay, Timeout.Infinite);
        }
    }
    
    private async Task HandleChange()
    {
        Recalculate();

        await _hub.Clients.All.SendAsync("translationsUpdated", new
        {
            version = _store.CurrentVersion
        });
    }

    private void Recalculate()
    {
        if ((DateTime.UtcNow - _lastRun).TotalMilliseconds < _fileChangesDelay)
        {
            return;
        }

        _lastRun = DateTime.UtcNow;

        var newVersion = _calculator.Calculate(_localizationFilesPath);

        if (_store.CurrentVersion != newVersion)
        {
            _store.SetCurrentVersion(newVersion);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _watcher.Dispose();
        
        return Task.CompletedTask;
    }
}