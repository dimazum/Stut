using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using stutvds.Logic.Services;

namespace stutvds.MiddleWares;

public class CultureCaptureMiddleware
{
    private readonly RequestDelegate _next;
    

    public CultureCaptureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentLanguage lang)
    {
        var path = context.Request.Path;

        var segments = path.Value?
            .Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments?.Length > 0)
        {
            var firstSegment = segments[0];

            if (StuConstants.Cultures.Contains(firstSegment))
            {
                lang.Culture = firstSegment;
            }
        }

        if (path == "/")
        {
            lang.Culture = StuConstants.DefaultCulture;
        }

        await _next(context);
    }
}