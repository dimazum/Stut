using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stutvds.Controllers.Base;
using stutvds.DAL.Entities;
using stutvds.DAL.Repositories;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistogramController : BaseController
{
    private readonly HistogramRepository _histogramRepository;
    private readonly IMapper _mapper;

    public HistogramController(HistogramRepository histogramRepository, IMapper mapper)
    {
        _histogramRepository = histogramRepository;
        _mapper = mapper;
    }

    [HttpPost("save")]
    public async Task<ActionResult> SaveHistogram([FromBody] HistogramDto histogram)
    {
        var entity = _mapper.Map<Histogram>(histogram);
        await _histogramRepository.AddOrUpdate(entity);
        return Ok();
    }

    [HttpPost("getOrCreateHistogram")]
    public async Task<ActionResult<HistogramDto>> GetOrCreateHistogram([FromBody] GetOrCreateHistogramDto dto)
    {
        var histogram = await _histogramRepository.GetWithCharmsByName(dto.Name);
        
        if (histogram == null)
        {
            histogram = await InitHistogram(dto.Name, dto.InitText);
            if (dto.SaveToDb)
            {
               // await _histogramRepository.AddOrUpdate(histogram);
            }
        }
        
        var histogramDto = _mapper.Map<HistogramDto>(histogram);
        return Ok(histogramDto);
    }
    
    [HttpGet("addcolumn")] 
    public async Task<ActionResult<HistogramDto>> AddColumn([FromQuery] string name, [FromQuery] int order)
    {
        var histogram = await _histogramRepository.GetWithCharmsByName(name);
        
        if (histogram == null)
        {
            return NotFound(name);
        }
        
        var needToReorderChars = histogram.Chars.Where(x => x.Order > order).ToList();

        foreach (var charItem in needToReorderChars)
        {
            charItem.Order++;
        }
        
        histogram.Chars.Add(new CharItem()
        {
            Char = string.Empty,
            Order = order + 1,
            Air = 100,
            HistogramId = histogram.Id
        });
        
        var entity = await _histogramRepository.AddOrUpdate(histogram);
        
        var dto = _mapper.Map<HistogramDto>(entity);
        return Ok(dto);
    }
    
    [HttpGet("removecolumn")] 
    public async Task<ActionResult<HistogramDto>> RemoveColumn([FromQuery] string name, [FromQuery] int order)
    {
        var histogram = await _histogramRepository.GetWithCharmsByName(name);
        
        if (histogram == null)
        {
            return NotFound(name);
        }
        
        var currentCharItem = histogram.Chars.FirstOrDefault(x => x.Order == order);
        
        histogram.Chars.Remove(currentCharItem);
        
        var needToReorderChars = histogram.Chars.Where(x => x.Order > order).ToList();

        foreach (var charItem in needToReorderChars)
        {
            charItem.Order--;
        }

        
        await _histogramRepository.UpdateAsync(histogram);
        
        var dto = _mapper.Map<HistogramDto>(histogram);
        return Ok(dto);
    }
    

    private async Task<Histogram> InitHistogram(string name, string initText)
    {
        var histogram = new Histogram()
        {
            Name = name,
            Chars = new List<CharItem>()
        };

        var order = 0;

        histogram.Chars.Add(new CharItem { Char = "", Air = 40, Order = order++ });
        histogram.Chars.Add(new CharItem { Char = "", Air = 80, Order = order++ });
        histogram.Chars.Add(new CharItem { Char = "", Air = 120, Order = order++ });
        histogram.Chars.Add(new CharItem { Char = "", Air = 112, Order = order++ });

        //Получаем список рун (Unicode-символов)
        var runes = initText.EnumerateRunes().ToList();

        int commaCount = runes.Count(r => r.Value == ',');

        float startPhraseAir = 112 - 8;

        var d = (startPhraseAir - 20 - (commaCount * 4)) / runes.Count;

        for (int i = 0; i < runes.Count; i++)
        {
            var currentChar = runes[i].ToString();
            var commaAir = 0;

            if (i + 1 < runes.Count)
            {
                var nextRune = runes[i + 1];

                if (nextRune.Value == ',' || nextRune.Value == '.')
                {
                    commaAir = 4;
                    currentChar += nextRune.ToString();
                    i++; // пропускаем запятую/точку
                }
            }

            histogram.Chars.Add(new CharItem()
            {
                Char = currentChar,
                Air = startPhraseAir,
                Order = order++
            });

            startPhraseAir = startPhraseAir - d - commaAir;
        }

        histogram.Chars.Add(new CharItem { Char = "", Air = 13.4f, Order = order++ });
        histogram.Chars.Add(new CharItem { Char = "", Air = 6.66f, Order = order });

        return histogram;
    }
}