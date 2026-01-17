using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL.Repositories.Contracts;
using stutvds.Integrations;
using stutvds.Logic.Services.Contracts;
using stutvds.Mappers;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : BaseController
    {
        private const string ArticlePath = "data/readData/ru";
        
        private readonly IArticleRepository _articleRepository;
        private readonly IWikiService _wikiService;
        private readonly IHttpContextAccessor _accessor;
        private readonly PollinationsIS _pollinationsIs;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ArticleController(IArticleRepository articleRepository,
            IWikiService wikiService,
            IHttpContextAccessor accessor,
            PollinationsIS pollinationsIS, IMapper mapper,
            IWebHostEnvironment env)
        {
            _articleRepository = articleRepository;
            _wikiService = wikiService;
            _accessor = accessor;
            _pollinationsIs = pollinationsIS;
            _mapper = mapper;
            _env = env;
        }
        
        [HttpGet]
        [Route("bytytle")]
        public JsonResult GetArticle(string title)
        {
            var article = _articleRepository.GetArticleByTitle(title);
            return new JsonResult(article);
        }

        [HttpGet("random")]
        public async Task<ActionResult<ArticleDto>> GetRandomArticle(string category)
        {
            if (category == null)
            {
                category = "Сказки";
            }
            
            category = ArticleCategoryMapper.ToEn(category);

            if ( category == "Countries")
            {
                var article = _articleRepository.GetRandomArticle(CurrentLanguage);
                var dtoWithoutCategory =  _mapper.Map<ArticleDto>(article);
                
                dtoWithoutCategory.Categories = GetCategories();
            
                return Ok(dtoWithoutCategory);
            }
            
            var folderPath = Path.Combine(_env.WebRootPath, ArticlePath, category);

            if (!Directory.Exists(folderPath))
                return NotFound("Category not found");

            var files = Directory.GetFiles(folderPath, "*.txt");

            if (files.Length == 0)
                return NotFound("No files in category");

            var randomFile = files[Random.Shared.Next(files.Length)];
            var text = System.IO.File.ReadAllText(randomFile);

            var fullName = Path.GetFileNameWithoutExtension(randomFile);
            var title = fullName.Split("_").First();
            var dto =  new ArticleDto()
            {
                Title = title,
                Content = text,
                Topic = category,
                Categories = GetCategories()
                
            };
                
            dto.Categories = GetCategories();
            
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
            var allCountries = _wikiService.GetAllTitles(CurrentLanguage);

            foreach (var country in allCountries)
            {
                var url = _wikiService.GetUrl(country, CurrentLanguage);

                if (!string.IsNullOrWhiteSpace(url))
                {
                    var article = _wikiService.GetArticle(url);
                    
                    string decodedUrl = Uri.UnescapeDataString(url);

                    _articleRepository.AddArticle(article, country, "Countries", decodedUrl, CurrentLanguage, AgeGroup.Adult);
                }

            }
        }

        private List<string> GetCategories()
        {
            var dataPath = Path.Combine(_env.WebRootPath, ArticlePath);

            if (!Directory.Exists(dataPath))
                return [];

            var categories = Directory.GetDirectories(dataPath)
                .Select(Path.GetFileName)
                .ToList();
            
            categories.Add(ArticleCategoryMapper.ToRu("Countries"));

            return categories;
        }
        

    }
}