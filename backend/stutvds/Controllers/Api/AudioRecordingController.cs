using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using stutvds.Controllers.Base;
using stutvds.DAL.Repositories;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AudioController : BaseController
{
    private readonly VoiceAnalyseRepository _voiceAnalyseRepository;
    private readonly string _uploadPath;

    public AudioController(IConfiguration config,
        IWebHostEnvironment env,
        VoiceAnalyseRepository voiceAnalyseRepository)
    {
        _voiceAnalyseRepository = voiceAnalyseRepository;
        var configuredPath = config["AudioStoragePath"];
        _uploadPath = Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.Combine(env.ContentRootPath, configuredPath);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is empty");
        
        var userFolder = Path.Combine(_uploadPath, UserId.ToString());
        Directory.CreateDirectory(userFolder);
        
        var timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm-ss");
        var fileName = $"{timestamp}{Path.GetExtension(file.FileName)}"; // например 30-11-2025_15-30-12.webm
        var filePath = Path.Combine(userFolder, fileName);

        await using var stream = System.IO.File.Create(filePath);
        await file.CopyToAsync(stream);

        return Ok(new { file = fileName, path = filePath, uploadedAt = DateTime.UtcNow });

        var t =
            "На утреннем поле медленно пробегала белка. Ветер осторожно шевелил высокие травы, а солнце освещало капли росы на каждом листочке. Проходя мимо старого дуба, птицы пели свои трели, переплетая лёгкую музыку с шумом листьев." +
            "\n\nКаждый звук природы словно напоминал о тишине и гармонии вокруг. Человек, остановившийся на мгновение, мог услышать дыхание леса, почувствовать ритм ветра и мягкость солнечного света. Шагая по тропинке, легко замечать, как каждое движение сопровождается мелодией, а простая прогулка превращается в маленькое приключение для слуха и души.";
    }
    
    // ============================================================
    //  GET api/audio/list
    //  Возвращает список файлов с датами
    // ============================================================
    [HttpGet("list")]
    public IActionResult List()
    {
        var userFolder = Path.Combine(_uploadPath, UserId.ToString());

        if (!Directory.Exists(userFolder))
            return Ok(new List<object>()); // нет файлов → пустой список

        var files = Directory
            .GetFiles(userFolder)
            .Select(f => new
            {
                FileName = Path.GetFileName(f),
                FullPath = f,
                UploadedAt = ParseTimestamp(Path.GetFileNameWithoutExtension(f))
            })
            .OrderByDescending(x => x.UploadedAt)
            .ToList();

        return Ok(files);
    }


    // ============================================================
    //  Возвращает сам аудиофайл
    // ============================================================
    [HttpGet("file/{fileName}")]
    public IActionResult GetFile(string fileName)
    {
        var userFolder = Path.Combine(_uploadPath, UserId.ToString());
        var filePath = Path.Combine(userFolder, fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound("File not found");

        var contentType = "audio/webm"; // можно сделать автоопределение

        var bytes = System.IO.File.ReadAllBytes(filePath);
        return File(bytes, contentType, fileName);
    }
    
    [HttpDelete("{fileName}")]
    public IActionResult DeleteFile(string fileName)
    {
        var userFolder = Path.Combine(_uploadPath, UserId.ToString());
        var filePath = Path.Combine(userFolder, fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound("File not found");

        System.IO.File.Delete(filePath);
        return Ok(new { message = "File deleted" });
    }

    // ============================================================
    // Парсер даты из имени файла: "dd-MM-yyyy_HH-mm-ss"
    // Например: 30-11-2025_15-30-12
    // ============================================================
    private DateTime ParseTimestamp(string fileBaseName)
    {
        if (DateTime.TryParseExact(
            fileBaseName,
            "dd-MM-yyyy_HH-mm-ss",
            null,
            System.Globalization.DateTimeStyles.AssumeUniversal,
            out var dt))
        {
            return dt;
        }

        return DateTime.MinValue;
    }
}