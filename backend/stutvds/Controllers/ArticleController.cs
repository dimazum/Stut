using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL.Repositories.Contracts;
using stutvds.Logic.Services.Contracts;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : BaseController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IWikiService _wikiService;
        private readonly IHttpContextAccessor _accessor;

        public ArticleController(IArticleRepository articleRepository, IWikiService wikiService, IHttpContextAccessor accessor)
        {
            _articleRepository = articleRepository;
            _wikiService = wikiService;
            _accessor = accessor;
        }
        
        [HttpGet]
        [Route("bytytle")]
        public JsonResult GetArticle(string title)
        {
            var article = _articleRepository.GetArticleByTitle(title);
            return new JsonResult(article);
        }

        [HttpGet]
        [Route("random")]
        public JsonResult GetRandomArticle()
        {
            var article = _articleRepository.GetRandomArticle(CurrentLanguage);
            return new JsonResult(article);
        }

        [HttpGet]
        [Route("countries")]
        public JsonResult GetCountries()
        {
            var countries = _articleRepository.GetArticleTitles(CurrentLanguage);
            return new JsonResult(countries);
        }
        
        [HttpGet]
        [Route("seed")]
        public void SeedAllArticles()
        {
            var t = _accessor;
            var allCountries = _wikiService.GetAllTitles(CurrentLanguage);

            foreach (var country in allCountries)
            {
                var url = _wikiService.GetUrl(country, CurrentLanguage);

                if (!string.IsNullOrWhiteSpace(url))
                {
                    var article = _wikiService.GetArticle(url);

                    _articleRepository.AddArticle(article, country, CurrentLanguage, AgeGroup.Adult);
                }

            }
        }
    }
}