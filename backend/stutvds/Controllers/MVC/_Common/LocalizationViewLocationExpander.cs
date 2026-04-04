using System.Collections.Generic;
using System.Linq;

namespace stutvds.Controllers.MVC._Common;

using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;

public class LocalizationViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context)
    {
        var routeCulture = context.ActionContext.RouteData.Values["culture"]?.ToString() ?? "en";
        context.Values["culture"] = routeCulture;
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        
        var routeCulture = context.ActionContext.RouteData.Values["culture"]?.ToString();

        var culture = !string.IsNullOrEmpty(routeCulture) ? routeCulture : CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        
        var locations = new[]
        {
            "/Controllers/MVC/{1}/" + culture + "/{0}.cshtml",
            "/Controllers/MVC/{1}/{0}.cshtml" // fallback ru
        };
        
    
        return locations.Concat(viewLocations);
    }
}