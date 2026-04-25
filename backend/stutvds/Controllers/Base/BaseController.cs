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
            { "en", Language.Russian },
            { "ru", Language.Russian },
            { "" , Language.Russian}
        };
        
        protected Guid UserId
        {
            get
            {
                var id =  HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (id == null)
                {
                    throw new StuException(ErrorCodes.Unauthorized.Message, ErrorCodes.Unauthorized.Code);
                }
                
                return Guid.Parse(id);
            }
        }
        
        protected string GetUserId()
        {
            var id = HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null) throw new StuException("User not logged in", 3);
            return id;
        }

        protected bool IsAuthenticated => HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated;
        

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
        
        protected BadRequestObjectResult BadRequest(ErrorDefinition errorDefinition)
        {
            var response = new
            {
                code = errorDefinition.Code,
                message = errorDefinition.Message
            };

            return base.BadRequest(response);
        }
    }
}