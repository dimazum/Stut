using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL.Repositories.Contracts;
using stutvds.Integrations;
using stutvds.Logic.Services.Contracts;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : BaseController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IWikiService _wikiService;
        private readonly IHttpContextAccessor _accessor;
        private readonly PollinationsIS _pollinationsIs;
        private readonly IMapper _mapper;

        public ArticleController(IArticleRepository articleRepository,
            IWikiService wikiService,
            IHttpContextAccessor accessor,
            PollinationsIS pollinationsIS, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _wikiService = wikiService;
            _accessor = accessor;
            _pollinationsIs = pollinationsIS;
            _mapper = mapper;
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
        public async Task<ActionResult<ArticleDto>> GetRandomArticle()
        {
            //var randomText = await _pollinationsIs. GetDubaiFact();

            var article = _articleRepository.GetRandomArticle(CurrentLanguage);
            var dto =  _mapper.Map<ArticleDto>(article);
            
            return Ok(dto);
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