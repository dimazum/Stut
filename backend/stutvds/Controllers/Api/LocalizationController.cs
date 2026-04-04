using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using stutvds.Logic.Services;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocalizationController : ControllerBase
{
    private readonly IStringLocalizer<Shared> _localizer;
    private readonly ICurrentLanguage _language;

    public LocalizationController(IStringLocalizer<Shared> localizer, ICurrentLanguage language)
    {
        _localizer = localizer;
        _language = language;
    }

    [HttpGet]
    public ActionResult<LocalizationDto> Get()
    {
        var ci = new CultureInfo(_language.Culture);

        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;

        var data = _localizer
            .GetAllStrings()
            .ToDictionary(x => x.Name, x => x.Value);

        var dto = new LocalizationDto()
        {
            Lang = _language.Culture,
            Translations = data
        };

        return Ok(dto);
    }
}