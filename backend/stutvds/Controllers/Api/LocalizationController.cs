using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using stutvds.Localization;
using stutvds.Logic.Services;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocalizationController : ControllerBase
{
    private readonly IStringLocalizer<Shared> _localizer;
    private readonly ICurrentLanguage _language;
    private readonly ILocalizationVersionStore _store;

    public LocalizationController(
        IStringLocalizer<Shared> localizer,
        ICurrentLanguage language,
        ILocalizationVersionStore store)
    {
        _localizer = localizer;
        _language = language;
        _store = store;
    }

    [HttpGet]
    public ActionResult<LocalizationDto> Get()
    {
        if (_language.Culture == null)
        {
            return Ok(new LocalizationDto
            {
                Version = _store.CurrentVersion
            });
        }
        
        var ci = new CultureInfo(_language.Culture);

        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;

        var data = _localizer
            .GetAllStrings()
            .ToDictionary(x => x.Name, x => x.Value);

        var dto = new LocalizationDto
        {
            Version = _store.CurrentVersion,
            Lang = _language.Culture,
            Translations = data
        };

        return Ok(dto);
    }
}