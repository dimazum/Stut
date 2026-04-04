using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;


namespace stutvds.Controllers.MVC._Common;
public class LanguagePickerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
        var currentCulture = requestCulture?.RequestCulture.UICulture.TwoLetterISOLanguageName ?? "ru";

        var model = new LanguagePickerModel
        {
            CurrentCulture = currentCulture,
            CurrentPath = HttpContext.Request.Path.Value ?? "/"
        };

        return View(model);
    }
}

public class LanguagePickerModel
{
    public string CurrentCulture { get; set; } = "ru";
    public string CurrentPath { get; set; } = "/";
}