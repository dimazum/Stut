using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace stutvds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocalizationController : ControllerBase
{
    private readonly IStringLocalizer _localizer;

    public LocalizationController(IStringLocalizerFactory factory)
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name!;
        _localizer = factory.Create("Shared", assemblyName);
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string culture)
    {
        var ci = new CultureInfo(culture);

        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;

        var data = _localizer
            .GetAllStrings()
            .ToDictionary(x => x.Name, x => x.Value);

        return Ok(data);
    }
}