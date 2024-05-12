using System;
using System.Collections.Generic;
using System.Globalization;
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
            { "en-US", Language.Russian },
            { "ru-RU", Language.Russian },
            { "" , Language.Russian}
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
                var t = CultureInfo.CurrentCulture.Name;
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