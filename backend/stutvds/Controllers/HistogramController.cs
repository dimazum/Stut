using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    private readonly Dictionary<string, string> _histograms = new Dictionary<string, string>()
    {
        { "Hist1",
            """
            Привет, это тестовое предложение. 
            Обрати внимание, что после вдоха, немного выдыхаем и плавно начинаем речь. 
            На паузах, как на коротких, так и на длинных, воздух плавно выдыхаем не задерживая. 
            На этой гистограмме видно как меняется количество воздуха в легких.
            """},
        { "Hist2",
            """
            Смотри, Вот так говорить нельзя. 
            Главное не блокировать голосовыми связками выдох.
            """}
    };


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

    [HttpGet("{name}")]
    public async Task<ActionResult<HistogramDto>> GetHistogram([FromQuery] string name)
    {
        var histogram = await _histogramRepository.GetWithCharmsByName(name);
        
        if (histogram == null)
        {
            histogram = await InitHistogram(name);
            histogram = await _histogramRepository.AddAsync(histogram);
        }
        
        var dto = _mapper.Map<HistogramDto>(histogram);
        return Ok(dto);
    }


    private async Task<Histogram> InitHistogram(string name)
    {
        var histogram = new Histogram()
        {
            Name = name,
            Chars = new List<CharItem>()
        };
        
        var phrase = _histograms[name].ToCharArray();
        
        histogram.Chars.Add(new CharItem()
        {
            Char = "",
            Air = 100
        });
        
        histogram.Chars.Add(new CharItem()
        {
            Char = "",
            Air = 100
        });

        for (var i = 0; i < phrase.Length; i++)
        {
            var currentChar = phrase[i].ToString();

            // проверяем, что есть следующий символ
            if (i + 1 < phrase.Length)
            {
                var nextChar = phrase[i + 1];

                if (nextChar == ',' || nextChar == '.')
                {
                    currentChar += nextChar.ToString();
                    i++; // пропускаем запятую/точку
                }
            }

            histogram.Chars.Add(new CharItem()
            {
                Char = currentChar,
                Air = 100
            });
        }

        
        histogram.Chars.Add(new CharItem()
        {
            Char = "",
            Air = 100
        });
        
        histogram.Chars.Add(new CharItem()
        {
            Char = "",
            Air = 100
        });

        return histogram;
    }

    // [HttpGet("all")]
    // public async Task<IActionResult> GetAllHistograms()
    // {
    //     var histograms = await _context.Histograms
    //         .Include(h => h.Chars)
    //         .ToListAsync();
    //     return Ok(histograms);
    // }
}