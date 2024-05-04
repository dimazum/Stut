using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;

namespace stutvds.Controllers.Base
{
    public class BaseController : Controller
    {
        private readonly Dictionary<string, Language> _cultureLanguageMap = new Dictionary<string, Language>()
        {
            { "en-EN", Language.English },
            { "ru-RU", Language.Russian }
        };
        
        protected Guid? UserId
        {
            get
            {
                var id =  HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (id == null)
                {
                    return null;
                }
                
                return Guid.Parse(id);
            }
        }

        protected Language CurrentLanguage
        {
            get
            {
                var requestCultureFeature = HttpContext?.Features.Get<IRequestCultureFeature>();

                if (requestCultureFeature != null)
                {
                    return _cultureLanguageMap[requestCultureFeature?.RequestCulture.UICulture.Name];
                }

                return Language.None;
            }
        }
    }
}